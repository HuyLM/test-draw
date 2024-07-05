using System;
using Falcon.FalconAnalytics.Scripts.Models.Attributes;
using Falcon.FalconAnalytics.Scripts.Models.Messages.Abstracts;
using Falcon.FalconCore.Scripts.Repositories;
using Falcon.FalconCore.Scripts.Repositories.News;
using UnityEngine.Scripting;

namespace Falcon.FalconAnalytics.Scripts.Models.Messages.PreDefines
{
    [Serializable]
    public class FSessionLog : BaseFalconLog
    {
        [FSortKey] public string gameMode;

        public int sessionId;
        public int sessionTime;
        public int currentLevel;
        public long modeTotalTime;
        [Preserve]
        public FSessionLog()
        {
        }

        public FSessionLog(string gameMode, TimeSpan sessionTime, int currentLevel = 0)
        {
            this.gameMode = gameMode;
            sessionId = FPlayerInfoRepo.SessionId;
            this.sessionTime = (int)sessionTime.TotalSeconds;
            this.currentLevel = currentLevel;
            modeTotalTime = FDataPool.Instance.Compute<long>("Session_Total_" + gameMode, (hasKey, val) =>
            {
                if (!hasKey) val = 0;
                return val + (long)sessionTime.TotalSeconds;
            });
        }

        public override string Event => "f_sdk_session_data";
        
    }
}