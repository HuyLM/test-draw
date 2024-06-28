using AtoGame.Base;
using AtoGame.OtherModules.LocalSaveLoad;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    [System.Serializable]
    public class LevelsSaveData : LocalSaveLoadable
    {
        private const string KEY = "LevelsSaveData";

        [JsonProperty] private List<LevelData> ls;

        [JsonIgnore]
        public List<LevelData> Levels
        {
            get => ls;
            set => ls = value;
        }

        public LevelData GetLevel(int index)
        {
            if(index < 0 || index >= Levels.Count)
            {
                return null;
            }
            return Levels[index];
        }

        public int GetCurrentUnlockLevelIndex()
        {
            for(int i = 0; i < Levels.Count; ++i)
            {
                if(Levels[i].State == LevelState.Unlocked)
                {
                    return i;
                }
            }
            return -1;
        }

        public int GetMinLockedLevelIndex()
        {
            for (int i = 0; i < Levels.Count; ++i)
            {
                if (Levels[i].State == LevelState.Locked)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool HasNextLevel(int levelIndex)
        {
            return levelIndex + 1 < Levels.Count;
        }

        public void UnlockNextLevel()
        {
            // check has any unlocked level
            int curUnlockedLevelIndex = GetCurrentUnlockLevelIndex();
            if(curUnlockedLevelIndex > 0) // have a unlocked level -> return;
            {
                return;
            }

            int lockedLevelIndex = GetMinLockedLevelIndex();
            if(lockedLevelIndex > 0)
            {
                Levels[lockedLevelIndex].UnlockLevel(false);
            }
        }

        public void UnlockLevel(int index)
        {
            Levels[index].ForceUnlockLevel(false);
            SaveData();
        }

        public void CheatUnlockAll()
        {
            foreach (var l in Levels)
            {
                l.ForceUnlockLevel(false);
            }
            SaveData();
        }

        #region LocalSaveLoadable
        public void CreateData()
        {
            var config = DataConfigs.Instance.LevelsConfigData;
            Levels = new List<LevelData>();
            for (int i = 0; i < config.Levels.Length; ++i)
            {
                Levels.Add(new LevelData(config.Levels[i]));
            }
            Levels[0].State = LevelState.Unlocked;
        }

        public void InitData()
        {
            bool hasUnlockedLevel = false;
            bool hasLockedLevel = false;
            var config = DataConfigs.Instance.LevelsConfigData;
            // add config
            for (int i = 0; i < Levels.Count; ++i)
            {
                Levels[i].SetConfig(config.Levels[i], i);
                if(Levels[i].State == LevelState.Unlocked)
                {
                    hasUnlockedLevel = true;
                }
                else if(Levels[i].State == LevelState.Locked)
                {
                    hasLockedLevel = true;
                }
            }

            // no unlocked levels and has least one locked level
            if (hasUnlockedLevel == false && hasLockedLevel == true)
            {
                int lockedLevelIndex = GetMinLockedLevelIndex();
                if (lockedLevelIndex >= 0)
                {
                    Levels[lockedLevelIndex].UnlockLevel(false);
                }
            }

        }

        public void LoadData()
        {
            string json = PlayerPrefExtension.GetString(KEY, string.Empty);
            if (string.IsNullOrEmpty(json) == true)
            {
                CreateData();
            }
            else
            {
                var temp = JsonConvert.DeserializeAnonymousType(json, this);
                Levels = temp.Levels;
                temp = null;

                var config = DataConfigs.Instance.LevelsConfigData;

                // validate data
                if (Levels.Count > config.Levels.Length)
                {
                    Debug.LogError($"NormalMapSaveData Validate Normal: Number of LevelSave > Number of StickerConfig");
                    return;
                }
                if (Levels.Count < config.Levels.Length)
                {
                    int oldSaveDataNumber = Levels.Count;
                    int diff = config.Levels.Length - oldSaveDataNumber;
                    for (int i = 0; i < diff; ++i)
                    {
                        Levels.Add(new LevelData(config.Levels[oldSaveDataNumber + i]));
                    }
                }
            }
        }

        public void SaveData()
        {
            string json = JsonConvert.SerializeObject(this);
            PlayerPrefExtension.SetString(KEY, json);
        }

        public void EaseData()
        {
            CreateData();
        }
        #endregion
    }
}
