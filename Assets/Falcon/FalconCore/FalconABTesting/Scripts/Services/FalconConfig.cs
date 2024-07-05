using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using Falcon.FalconCore.FalconABTesting.Scripts.Payloads;
using Falcon.FalconCore.FalconABTesting.Scripts.Repositories;
using Falcon.FalconCore.Scripts.Controllers.Interfaces;
using Falcon.FalconCore.Scripts.Logs;
using Falcon.FalconCore.Scripts.Services.MainThreads;
using Falcon.FalconCore.Scripts.Utils.Entities;
using Falcon.FalconCore.Scripts.Utils.FActions.Variances.Starts;
using UnityEngine.Scripting;

namespace Falcon.FalconCore.Scripts.FalconABTesting.Scripts.Model
{
    public abstract class FalconConfig
    {
        private static readonly FConcurrentDict<Type, FalconConfig> Cache = new FConcurrentDict<Type, FalconConfig>();

        private static readonly object Locker = new object();

        private static string _abTestString;
        public static ExecState UpdateFromNet { get; private set; } = ExecState.NotStarted;
        public static string RunningAbTesting => FalconConfigRepo.RunningAbTesting;

        public static string AbTestingString
        {
            get
            {
                if (_abTestString == null)
                    lock (Locker)
                    {
                        var builder = new StringBuilder();
                        foreach (var configObj in FalconConfigRepo.TestingConfigs)
                            builder.Append(Convert.ToString(configObj.name, CultureInfo.InvariantCulture))
                                .Append(":")
                                .Append(Convert.ToString(configObj.Value, CultureInfo.InvariantCulture))
                                .Append("_");
                        if (builder.Length > 0) builder.Length--;

                        _abTestString = builder.ToString();
                    }

                return _abTestString;
            }
        }

        public static event EventHandler OnUpdateFromNet;

        public static T Instance<T>() where T : FalconConfig, new()
        {
            return (T)Cache.Compute(typeof(T), (hasKey, config) =>
            {
                if (hasKey) return config;
                return CreateInstance<T>();
            });
        }

        private static T CreateInstance<T>() where T : FalconConfig, new()
        {
            var result = new T();
            foreach (var configObject in FalconConfigRepo.RemoteConfigs)
                try
                {
                    var info = typeof(T).GetField(configObject.name,BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
                    if (info != null) info.SetValue(result, Convert.ChangeType(configObject.Value, info.FieldType));
                }
                catch (Exception)
                {
                    //ignored
                }

            return result;
        }

        [Preserve]
        private sealed class FConfigInit : IFInit
        {
            [Preserve]
            public FConfigInit()
            {
            }

            public IEnumerator Init()
            {
                var webPull = new UnitAction(UpdateFromWeb);
                webPull.Schedule();
                while (!webPull.Done) yield return null;
            }

            private static void UpdateFromWeb()
            {
                var sendConfig = new SendConfig();

                try
                {
                    lock (Locker)
                    {
                        UpdateFromNet = ExecState.Processing;
                        var receiveConfig = sendConfig.Connect();
                        UpdateFromNet = ExecState.Succeed;
                        FalconConfigRepo.Save(receiveConfig);
                        _abTestString = null;
                        Cache.Clear();
                    }

                    new MainThreadAction(() => { OnUpdateFromNet?.Invoke(null, EventArgs.Empty); }).Schedule();
                }
                catch (Exception e)
                {
                    CoreLogger.Instance.Error(e);
                    UpdateFromNet = ExecState.Failed;
                }
            }
        }
    }
}