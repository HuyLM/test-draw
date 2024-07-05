using System;
using System.Threading;
using Falcon.FalconAnalytics.Scripts.Models.Attributes;
using Falcon.FalconAnalytics.Scripts.Models.Messages.Abstracts;
using Falcon.FalconAnalytics.Scripts.Payloads;
using Falcon.FalconAnalytics.Scripts.Payloads.Flex;
using Falcon.FalconAnalytics.Scripts.Services;
using Falcon.FalconCore.Scripts.FalconABTesting.Scripts.Model;
using Falcon.FalconCore.Scripts.Repositories;
using Falcon.FalconCore.Scripts.Repositories.News;
using Falcon.FalconCore.Scripts.Services.GameObjs;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Falcon.FalconAnalytics.Scripts.Models.Messages.PreDefines
{
    [Serializable]
    public class FActionLog : BaseFalconLog
    {
        [JsonIgnore] private static ActionLogConfig _actionLogConfig = FalconConfig.Instance<ActionLogConfig>();
        [JsonIgnore] private static readonly string SessionGuid = Guid.NewGuid().ToString();

        [FSortKey] public string aFrom;
        [FSortKey] public string aTo;

        public int aTime;
        public int sessionId;
        public string sessionUid = SessionGuid;

        static FActionLog()
        {
            FalconConfig.OnUpdateFromNet += (sender, args) => _actionLogConfig = FalconConfig.Instance<ActionLogConfig>();
            FGameObj.OnGameContinue += (sender, args) =>
                new FActionLog("GameContinue").Send();
            FGameObj.OnGameStop += (sender, args) =>
            {
                var flexLogWrapper = new FActionLog("GameStop");
                new Thread(() =>
                {
                    try
                    {
                        new DataWrapper(flexLogWrapper).Send();
                    }
                    catch (Exception e)
                    {
                        AnalyticLogger.Instance.Warning(e.Message);
                    }
                }).Start();
            };
        }

        [Preserve]
        public FActionLog()
        {
        }

        public FActionLog(string actionName)
        {
            if (LastActionTime == -1) LastActionTime = PlayTimeCheckService.SessionStartTime;
            aFrom = LastAction;
            aTo = actionName;

            aTime = (int)(FTime.CurrentTimeSec() - LastActionTime);
            sessionId = FPlayerInfoRepo.SessionId;

            LastActionTime = FTime.CurrentTimeSec();
            LastAction = aTo;
        }

        [JsonIgnore] private static string LastAction { get; set; } = "GameStart";

        [JsonIgnore] private static long LastActionTime { get; set; } = -1;

        public override string Event => "f_sdk_action_data";

        public override void Send()
        {
            if (_actionLogConfig.fCoreAnalyticActionLogEnable)
            {
                base.Send();
            }
            
        }
    }
}