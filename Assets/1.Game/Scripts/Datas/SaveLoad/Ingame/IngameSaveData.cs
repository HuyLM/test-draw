using AtoGame.Base;
using AtoGame.OtherModules.LocalSaveLoad;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class IngameSaveData : LocalSaveLoadable
    {
        private const string KEY = "IngameSaveData";


        #region LocalSaveLoadable
        public void CreateData()
        {
        }

        public void EaseData()
        {
            CreateData();
        }

        public void InitData()
        {
            string json = PlayerPrefExtension.GetString(KEY, string.Empty);
            if (string.IsNullOrEmpty(json) == true)
            {
                CreateData();
                return;
            }
            else
            {
                var temp = JsonConvert.DeserializeObject<IngameSaveData>(json);
            }
        }

        public void LoadData()
        {
        }

        public void SaveData()
        {
            string json = JsonConvert.SerializeObject(this);
            PlayerPrefExtension.SetString(KEY, json);
        }
        #endregion
    }
}
