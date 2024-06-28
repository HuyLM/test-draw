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
        [SerializeField] private Button btnNext;
        [SerializeField] private Button btnPrevious;
        [SerializeField] private float delay;

        protected override void Start()
        {
            base.Start();
            tsHaptics.OnChangeSwitch = OnHapticSwitchChanged;
            tsSound.OnChangeSwitch = OnSoundSwitchChanged;
            tsMusic.OnChangeSwitch = OnMusicSwitchChanged;
            btnNext.onClick.AddListener(OnNextButtonClicked);
            btnPrevious.onClick.AddListener(OnPreviousButtonClicked);
        }

        protected override void ActiveFrame()
        {
            base.ActiveFrame();
            btnNext.interactable = true;
            btnPrevious.interactable = true;
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

        private void OnNextButtonClicked()
        {
            btnNext.interactable = false;
            btnPrevious.interactable = false;

            DOVirtual.DelayedCall(delay, () => {
                btnNext.interactable = true;
                btnPrevious.interactable = true;
            });
            
            int languageNumber = LocalizationSettings.AvailableLocales.Locales.Count;
            int selected = 0;
            for(int i = 0; i < languageNumber; ++i)
            {
                var locale = LocalizationSettings.AvailableLocales.Locales[i];
                if(LocalizationSettings.SelectedLocale == locale)
                    selected = i;
            }
            int nextIndex = (selected + 1) % languageNumber;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[nextIndex];
        }

        private void OnPreviousButtonClicked()
        {
            btnNext.interactable = false;
            btnPrevious.interactable = false;

            DOVirtual.DelayedCall(delay, () => {
                btnNext.interactable = true;
                btnPrevious.interactable = true;
            });


            int languageNumber = LocalizationSettings.AvailableLocales.Locales.Count;
            int selected = 0;
            for(int i = 0; i < languageNumber; ++i)
            {
                var locale = LocalizationSettings.AvailableLocales.Locales[i];
                if(LocalizationSettings.SelectedLocale == locale)
                    selected = i;
            }

            int nextIndex = selected - 1;
            if(nextIndex < 0)
            {
                nextIndex = languageNumber - 1;
            }
            else
            {
                nextIndex  = nextIndex % languageNumber;
            }
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[nextIndex];
        }
    }
}
