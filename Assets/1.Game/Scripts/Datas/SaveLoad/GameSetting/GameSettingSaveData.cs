using AtoGame.Base;
using AtoGame.OtherModules.LocalSaveLoad;
using AtoGame.OtherModules.SoundManager;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class GameSettingSaveData : LocalSaveLoadable, SoundMangerSaver
    {
        private const string KEY = "GameSettingSaveData";

        [JsonProperty] private float sv;
        [JsonProperty] private bool se;
        [JsonProperty] private float mv;
        [JsonProperty] private bool me;

        [JsonProperty] private bool uh;

        [JsonIgnore]
        public float SoundVolume
        {
            get => sv;
            set => sv = value;
        }
        [JsonIgnore]
        public bool SoundEnable
        {
            get => se;
            set => se = value;
        }
        [JsonIgnore]
        public float MusicVolume
        {
            get => mv;
            set => mv = value;
        }
        [JsonIgnore]
        public bool MusicEnable
        {
            get => me;
            set => me = value;
        }
        [JsonIgnore]
        public bool UseHaptic
        {
            get => uh;
            set => uh = value;
        }
      


        #region LocalSaveLoadable
        public void CreateData()
        {
            SoundEnable = true;
            MusicEnable = true;
            SoundVolume = 0.5f;
            MusicVolume = 0.5f;
            UseHaptic = true;
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
                return;
            }
            else
            {
                var temp = JsonConvert.DeserializeObject<GameSettingSaveData>(json);
                SoundEnable = temp.SoundEnable;
                MusicEnable = temp.MusicEnable;
                SoundVolume = temp.SoundVolume;
                MusicVolume = temp.MusicVolume;
                UseHaptic = temp.UseHaptic;
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
            CreateData();
        }
        #endregion

        #region SoundManager Saver
        public void SetMasterEnable(bool enable)
        {
        }

        public void SetMasterVolume(float volume)
        {

        }

        public bool GetMasterEnable()
        {
            return true;
        }

        public float GetMasterVolume()
        {
            return 1;
        }

        public void SetMusicEnable(bool enable)
        {
            MusicEnable = enable;
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = volume;
        }

        public bool GetMusicEnable()
        {
            return MusicEnable;
        }

        public float GetMusicVolume()
        {
            return MusicVolume;
        }

        public void SetSFXEnable(bool enable)
        {
            SoundEnable = enable;
        }

        public void SetSFXVolume(float volume)
        {
            SoundVolume = volume;
        }

        public bool GetSFXEnable()
        {
            return SoundEnable;
        }

        public float GetSFXVolume()
        {
            return SoundVolume;
        }

        public void SetOtherEnable(string id, bool enable)
        {

        }

        public void SetOtherVolume(string id, float volume)
        {
        }

        public bool GetOtherEnable(string id)
        {
            return false;
        }

        public float GetOtherVolume(string id)
        {
            return 0;
        }

        #endregion
    }
}
