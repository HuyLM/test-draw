using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Falcon.FalconCore.FalconABTesting.Scripts.Models;
using Falcon.FalconCore.FalconABTesting.Scripts.Payloads;
using Falcon.FalconCore.Scripts.Repositories;

namespace Falcon.FalconCore.FalconABTesting.Scripts.Repositories
{
    public static class FalconConfigRepo
    {
        private const string Config = "FALCON_CONFIG";
        private static readonly object Locker = new object();

        private static ReceiveConfig _receiveConfig = FDataPool.Instance.GetOrSet(Config, new ReceiveConfig());
        
        private static ReadOnlyCollection<ConfigObject> _testingConfig;
        private static ReadOnlyCollection<ConfigObject> _nonTestConfig;
        public static ReceiveConfigObject[] RemoteConfigs => _receiveConfig.configs ??= Array.Empty<ReceiveConfigObject>();

        public static Dictionary<string, bool> CampaignMeta => _receiveConfig.CampaignMeta ??= new Dictionary<string, bool>();
        public static ReadOnlyCollection<ConfigObject> TestingConfigs
        {
            get
            {
                if (_testingConfig == null)
                {
                    lock (Locker)
                    {
                        List<ConfigObject> result = new List<ConfigObject>();
                        foreach (var receiveConfigConfig in _receiveConfig.configs)
                        {
                            if (receiveConfigConfig.abTesting)
                            {
                                result.Add(new ConfigObject(receiveConfigConfig));
                            }
                        }

                        _testingConfig = result.AsReadOnly();
                    }
                }
                return _testingConfig;
            }
        }

        public static ReadOnlyCollection<ConfigObject> NonTestConfigs
        {
            get
            {
                if (_nonTestConfig == null)
                {
                    lock (Locker)
                    {
                        List<ConfigObject> result = new List<ConfigObject>();
                        foreach (var receiveConfigConfig in _receiveConfig.configs)
                        {
                            if (!receiveConfigConfig.abTesting)
                            {
                                result.Add(new ConfigObject(receiveConfigConfig));
                            }
                        }

                        _nonTestConfig = result.AsReadOnly();
                    }
                }
                return _nonTestConfig;
            }
        }

        public static string RunningAbTesting => _receiveConfig.runningAbTesting;

        public static void Save(ReceiveConfig config)
        {
            lock (Locker)
            {
                _receiveConfig = config;
                _testingConfig = null;
                _nonTestConfig = null;
            }

            FDataPool.Instance.Save(Config, config);
        }
    }
}