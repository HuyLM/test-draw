using System;
using Falcon.FalconAnalytics.Scripts.Models.Messages.PreDefines;
using Falcon.FalconCore.Scripts.Controllers;
using Falcon.FalconCore.Scripts.Repositories;
using Falcon.FalconCore.Scripts.Repositories.News;
using UnityEngine;

namespace Falcon.FalconAnalytics.Scripts.Services
{
    public static class RetentionCheckService
    {
        private const string LatestLoginDateKey = "LATEST_DATE";

        public static int Retention =>
            DateTime.Compare(DateTime.Now.Date, FirstLoginDate.Date) > 0
                ? (DateTime.Now.Date - FirstLoginDate.Date).Days
                : 0;

        public static bool RetentionChanged =>
            DateTime.Compare(DateTime.Now.Date,
                FDataPool.Instance.GetOrDefault(LatestLoginDateKey, FirstLoginDate).Date) > 0;

        public static DateTime FirstLoginDate => FPlayerInfoRepo.FirstLoginDate;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Init()
        {
            Refresh();
            FDataPool.Instance.Save(LatestLoginDateKey, DateTime.Now);
            AnalyticLogger.Instance.Info("RetentionCheckService init complete");

        }

        public static void Refresh()
        {
            FDataPool.Instance.Compute<DateTime>(LatestLoginDateKey, (hasKey, latestLogin) =>
            {
                if (!hasKey)
                {
                    
                    new WaitInit(() => new FRetentionLog(Retention, FirstLoginDate).Send())
                        .Schedule();
                    return FirstLoginDate;
                }
                if (RetentionChanged)
                    new WaitInit(() => new FRetentionLog(Retention, FirstLoginDate).Send())
                        .Schedule();

                return latestLogin;
            });
        }
    }
}