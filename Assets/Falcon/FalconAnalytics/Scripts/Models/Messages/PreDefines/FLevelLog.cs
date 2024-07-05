﻿using System;
using Falcon.FalconAnalytics.Scripts.Enum;
using Falcon.FalconAnalytics.Scripts.Models.Attributes;
using Falcon.FalconAnalytics.Scripts.Models.Messages.Abstracts;
using Falcon.FalconCore.Scripts.Repositories;
using Falcon.FalconCore.Scripts.Repositories.News;
using UnityEngine.Scripting;

namespace Falcon.FalconAnalytics.Scripts.Models.Messages.PreDefines
{
    [Serializable]
    public class FLevelLog : BaseFalconLog
    {
        [FSortKey] public int currentLevel;
        [FSortKey] public string difficulty;
        [FSortKey] public LevelStatus status;

        /// <summary>
        ///     in second
        /// </summary>
        public int duration;
        public int wave;
        
        public int failCount;
        public int playCount;

        [Preserve]
        public FLevelLog()
        {
        }

        public FLevelLog(int currentLevel, string difficulty, LevelStatus status, TimeSpan duration, int wave = 0)
        {
            this.currentLevel = CheckNumberNonNegative(currentLevel, nameof(currentLevel));
            this.difficulty = difficulty;
            this.status = status;
            this.duration = (int)duration.TotalSeconds;
            this.wave = wave;
        }

        private void GetRightStatus()
        {
            FDataPool.Instance.Compute<long?>("HasPassedLevel_" + LevelId,
                (hasKey, val) =>
                {
                    if (hasKey)
                    {
                        if (status == LevelStatus.Fail)
                            status = LevelStatus.ReplayFail;
                        else if (status == LevelStatus.Pass) status = LevelStatus.ReplayPass;

                        return val;
                    }

                    if (LevelStatus.Pass.Equals(status))
                    {
                        FPlayerInfoRepo.MaxPassedLevel = currentLevel;
                        return FTime.CurrentTimeSec();
                    }

                    return null;
                });
        }

        private void SetCounting()
        {
            failCount = FDataPool.Instance.Compute<int>(
                "Fail_Count_Level_" + LevelId,
                (hasKey, val) =>
                {
                    if (!hasKey) val = 0;
                    if (status == LevelStatus.Fail || status == LevelStatus.ReplayFail) return val + 1;
                    return val;
                });
            playCount = FDataPool.Instance.Compute<int>(
                "Play_Count_Level_" + LevelId,
                (hasKey, val) =>
                {
                    if (!hasKey) val = 0;
                    return val + 1;
                });
        }
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected virtual String LevelId => currentLevel + "_Difficulty_" + difficulty;

        public override string Event => "f_sdk_level_data";
        
        public override void Send()
        {
            GetRightStatus();
            level = FPlayerInfoRepo.MaxPassedLevel;
            SetCounting();
            base.Send();
        }
    }
}