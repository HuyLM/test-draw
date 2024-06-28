using AtoGame.Base;
using AtoGame.OtherModules.LocalSaveLoad;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class AdsSaveData : LocalSaveLoadable
    {
        private const string KEY = "AdsSaveData";

        [JsonProperty] private bool ira;
        [JsonProperty] private bool ifsaoa;

        [JsonIgnore]
        public bool IsRemoveAds
        {
            get => ira;
            set => ira = value;
        }
        [JsonIgnore]
        public bool IsFirstShowAOA
        {
            get => ifsaoa;
            set => ifsaoa = value;
        }

        #region LocalSaveLoadable
        public void CreateData()
        {
            IsRemoveAds = false;
            IsFirstShowAOA = true;
        }
        public void InitData()
        {
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
                AdsSaveData temp = JsonConvert.DeserializeObject<AdsSaveData>(json);
                IsRemoveAds = temp.IsRemoveAds;
                IsFirstShowAOA = temp.IsFirstShowAOA;
                temp = null;
            }
        }

        public void SaveData()
        {
            string json = JsonConvert.SerializeObject(this);
            PlayerPrefExtension.SetString(KEY, json);
        }

        public void EaseData()
        {

        }
        #endregion
    }
}