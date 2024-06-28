using AtoGame.OtherModules.LocalSaveLoad;
//using AtoGame.OtherModules.OptimizedScrollView;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public enum LevelState
    {
        Unlocked, Locked, Completed
    }

    [System.Serializable]
    public class LevelData //: IScrollViewItemModel
    {
        [JsonProperty] private LevelState s;

        [JsonIgnore]
        public LevelState State
        {
            get => s;
            set => s = value;
        }

        [JsonIgnore]
        public int Index;


        [JsonIgnore] public LevelConfig Config;
        [JsonIgnore] public bool IsNativeAd;
        [JsonIgnore] public bool UseRegisterNativeAd;
        [JsonIgnore] public float PlayDurantion;


        public LevelData()
        {

        }

        public LevelData(LevelConfig config)
        {
            State = LevelState.Locked;
        }

        public void SetConfig(LevelConfig config, int index)
        {
            Config = config;
            Index = index;
        }

        public void ForceUnlockLevel(bool needSave)
        {
            State = LevelState.Unlocked;
            if (needSave)
            {
                LocalSaveLoadManager.Get<LevelsSaveData>().SaveData();
            }
        }

        public void UnlockLevel(bool needSave)
        {
            if(State == LevelState.Locked)
            {
                GameTracking.LogLevel_Unlock(Index);
                State = LevelState.Unlocked;
                if (needSave)
                {
                    LocalSaveLoadManager.Get<LevelsSaveData>().SaveData();
                }
            }
        }

        public void SkipLevel(bool needSave)
        {
            if(State == LevelState.Unlocked)
            {
                State = LevelState.Completed;
                if(needSave)
                {
                    LocalSaveLoadManager.Get<LevelsSaveData>().SaveData();
                }
                GameTracking.LogLevel_Skip(Index);
            }
        }

        public void CompleteLevel()
        {
            if(State == LevelState.Unlocked)
            {
                State = LevelState.Completed;
                var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
                saveData.UnlockNextLevel();
                saveData.SaveData();
                GameTracking.LogLevelAchieved(Index, 0, false);
            }
            else if (State == LevelState.Completed)
            {
                GameTracking.LogLevelAchieved(Index, 0, true);
            }
            GameTracking.LogLevelComplete(Index);
            GameTracking.LogLevel_Win(Index);
        }
    }
}
