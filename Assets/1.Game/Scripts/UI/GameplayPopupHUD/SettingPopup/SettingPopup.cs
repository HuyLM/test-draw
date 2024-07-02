using AtoGame.Base.UI;
using AtoGame.OtherModules.HUD;
using AtoGame.OtherModules.LocalSaveLoad;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace TrickyBrain
{
    public class SettingPopup : BasePopup
    {
        [SerializeField] private ToggleSwitch tsHaptics;
        [SerializeField] private ToggleSwitch tsSound;
        [SerializeField] private ToggleSwitch tsMusic;

        protected override void Start()
        {
            base.Start();
            tsHaptics.OnChangeSwitch = OnHapticSwitchChanged;
            tsSound.OnChangeSwitch = OnSoundSwitchChanged;
            tsMusic.OnChangeSwitch = OnMusicSwitchChanged;
        }

        protected override void ActiveFrame()
        {
            base.ActiveFrame();
            var saveData = LocalSaveLoadManager.Get<GameSettingSaveData>();
            tsHaptics.ForceSetState(saveData.UseHaptic);
            tsSound.ForceSetState(saveData.SoundEnable);
            tsMusic.ForceSetState(saveData.MusicEnable);
        }

        private void OnHapticSwitchChanged(bool state)
        {
            var saveData = LocalSaveLoadManager.Get<GameSettingSaveData>();
            saveData.UseHaptic = state;
            saveData.SaveData();
        }

        private void OnSoundSwitchChanged(bool state)
        {
            var saveData = LocalSaveLoadManager.Get<GameSettingSaveData>();
            if(state == true)
            {
                GameSoundManager.Instance.SFXVolume = 0.5f;
            }
            else
            {
                GameSoundManager.Instance.SFXVolume = 0.0f;
            }
            saveData.SaveData();
        }

        private void OnMusicSwitchChanged(bool state)
        {
            var saveData = LocalSaveLoadManager.Get<GameSettingSaveData>();
            GameSoundManager.Instance.MusicEnable = state;
            saveData.SaveData();
            if(state)
            {
                GameSoundManager.Instance.PlayGameplayBackground(true, 1);
            }
            else
            {
                GameSoundManager.Instance.StopMusic();
            }
        }
    }
}
