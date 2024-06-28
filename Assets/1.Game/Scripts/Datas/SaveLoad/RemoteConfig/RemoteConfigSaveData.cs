using AtoGame.Base;
using AtoGame.OtherModules.LocalSaveLoad;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class RemoteConfigSaveData : LocalSaveLoadable
    {
        private const string KEY = "RemoteConfigSaveData";

        [JsonProperty] private float iac;

        [JsonIgnore]
        public float InterstitialAdCapping
        {
            get => iac;
            set => iac = value;
        }

        #region LocalSaveLoadable
        public void CreateData()
        {
            InterstitialAdCapping = AppSeasonData.InterstitialCapping;
        }
        public void InitData()
        {

        }

        public void LoadData()
        {
            string json = PlayerPrefExtension.GetString(KEY, string.Empty);
            if(string.IsNullOrEmpty(json) == true)
            {
                CreateData();
            }
            else
            {
                RemoteConfigSaveData temp = JsonConvert.DeserializeObject<RemoteConfigSaveData>(json);
                InterstitialAdCapping = temp.InterstitialAdCapping;
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
