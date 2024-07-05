using System;
using System.Threading;
using Falcon.FalconAnalytics.Scripts.Models.Messages.PreDefines;
using Falcon.FalconAnalytics.Scripts.Payloads.Flex;
using Falcon.FalconCore.Scripts.Repositories;
using Falcon.FalconCore.Scripts.Services.GameObjs;
using UnityEngine;

namespace Falcon.FalconAnalytics.Scripts.Services
{
    public static class PlayTimeCheckService
    {
        private const string TotalTimeKey = "USER_TOTAL_TIME";
        private static long _startTime;
        public static long SessionStartTime { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {
            _startTime = FTime.CurrentTimeSec();
            SessionStartTime = _startTime;
            FGameObj.OnGameStop += (a, b) => SaveTotalTime();
            FGameObj.OnGameContinue += (a, b) => _startTime = FTime.CurrentTimeSec();
            AnalyticLogger.Instance.Info("PlayTimeCheckService init complete");
        }

        private static void SaveTotalTime()
        {
            FSessionLog sessionLog =
                new FSessionLog(TotalTimeKey, TimeSpan.FromSeconds(FTime.CurrentTimeSec() - _startTime));
            new Thread(() =>
            {
                try
                {
                    new DataWrapper(sessionLog).Send();
                }
                catch (Exception e)
                {
                    AnalyticLogger.Instance.Warning(e.Message);
                }
            }).Start();

            AnalyticLogger.Instance.Info("Current session time : " + (FTime.CurrentTimeSec() - _startTime));
        }
    }
}