using AtoGame.Base;
using AtoGame.OtherModules.LocalSaveLoad;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class ShopSaveData : LocalSaveLoadable
    {
        private const string KEY = "ShopSaveData";

        [JsonProperty] private int uii;

        [JsonIgnore]
        public int UsingItemID
        {
            get => uii;
            set => uii = value;
        }


        #region LocalSaveLoadable
        public void CreateData()
        {
            var config = DataConfigs.Instance.ShopConfigData;
            UsingItemID = config.GetDefaultItem().Id;
        }
        public void InitData()
        {
            var config = DataConfigs.Instance.ShopConfigData;
            config.UnlockDefaultItems();
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
                ShopSaveData temp = JsonConvert.DeserializeObject<ShopSaveData>(json);
                this.UsingItemID = temp.UsingItemID;
            }
        }

        public void SaveData()
        {
            string json = JsonConvert.SerializeObject(this);
            PlayerPrefExtension.SetString(KEY, json);
        }

        public void EaseData()
        {
            var config = DataConfigs.Instance.ShopConfigData;
            UsingItemID = config.GetDefaultItem().Id;
        }
        #endregion
    }
}
