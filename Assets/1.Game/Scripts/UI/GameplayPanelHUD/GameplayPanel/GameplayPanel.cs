using AtoGame.OtherModules.HUD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AtoGame.Base;
using AtoGame.Base.Utilities;
using AtoGame.OtherModules.LocalSaveLoad;
using static TrickyBrain.EventKey;
using UnityEngine.Localization;
using AtoGame.Base.Helper;
using AtoGame.Base.UI;
using System;

namespace TrickyBrain
{
    public class GameplayPanel : BasePanel
    {
        [SerializeField] private Button btnSetting;
        [SerializeField] private Button btnSkipLevel;
        [SerializeField] private Button btnHint;
        [SerializeField] private Button btnShop;
        [SerializeField] private TextMeshProUGUI txtLevel;
        [SerializeField] private TextMeshProUGUI txtStar;


        private int curLevelIndex;
        public int CurLevelIndex => curLevelIndex;


        private void Start()
        {
            btnSetting.onClick.AddListener(OnSettingButtonClicked);
            btnSkipLevel.onClick.AddListener(OnSkipButtonClicked);
            btnHint.onClick.AddListener(OnHintButtonClicked);
            btnShop.onClick.AddListener(OnShopButtonClicked);
        }
        private void OnEnable()
        {
            EventDispatcher.Instance.AddListener<LoadedLevelEvent>(OnLoadedLevel);
            EventDispatcher.Instance.AddListener<StartLevelEvent>(OnStartLevel);
            EventDispatcher.Instance.AddListener<EndLevelEvent>(OnEndLevel);
            EventDispatcher.Instance.AddListener<IgnoreInputEvent>(OnIgnoreInput);

        }

        private void OnDisable()
        {
            if(EventDispatcher.Initialized)
            {
                EventDispatcher.Instance.RemoveListener<LoadedLevelEvent>(OnLoadedLevel);
                EventDispatcher.Instance.RemoveListener<StartLevelEvent>(OnStartLevel);
                EventDispatcher.Instance.RemoveListener<EndLevelEvent>(OnEndLevel);
                EventDispatcher.Instance.RemoveListener<IgnoreInputEvent>(OnIgnoreInput);

            }
        }

        protected override void ActiveFrame()
        {
            base.ActiveFrame();
            btnSetting.interactable = false;
            btnHint.interactable = false;
            txtStar.text = "100";
        }

        protected override void OnShowedFrame()
        {
            base.OnShowedFrame();
            btnSetting.interactable = true;
            btnHint.interactable = true;

            
        }

        private void OnLoadedLevel(LoadedLevelEvent param)
        {
            SetLevelText(param.LevelIndex);
            SetLevelTitle(param.LevelIndex);
        }

        private void OnStartLevel(StartLevelEvent param)
        {
          
        }

        private void OnEndLevel(EndLevelEvent param)
        {
        }

        public void SetLevelText(int levelIndex)
        {
            curLevelIndex = levelIndex;
            txtLevel.text = $"LEVEL {levelIndex + 1}";

            var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
            btnSkipLevel.interactable = saveData.HasNextLevel(curLevelIndex);
        }

        private void SetLevelTitle(int levelIndex)
        {
            var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
            var levelConfig = saveData.GetLevel(levelIndex).Config;

            //txtTitle.text =levelConfig.TitleKey;
        }

        private void OnSettingButtonClicked()
        {
            PopupHUD.Instance.Show<SettingPopup>();
        }
       

        private void OnSkipButtonClicked()
        {
            if (AdsManager.Instance.IsRewardVideoReady())
            {
                GameTracking.LogShowAds(true, "feature_skip");
                AdsManager.Instance.ShowRewardVideo("Ingame_Skip", () => {
                    Skip();
                }, ()=> {
                    GameTracking.LogShowAds(false, "feature_skip");
                    string message = "key_show_ad_failed_message";
                    string title = "key_show_ad_failed_title";
                    NoticePopup noticePopup = PopupHUD.Instance.Show<NoticePopup>();
                    noticePopup.SetMessage(message);
                    noticePopup.SetTitle(title);
                });
            }
            else
            {
                GameTracking.LogShowAds(false, "feature_skip");
                string message = "key_ad_not_availiable_message";
                string title = "key_ad_not_availiable_title";
                NoticePopup noticePopup = PopupHUD.Instance.Show<NoticePopup>();
                noticePopup.SetMessage(message);
                noticePopup.SetTitle(title);
            }
        }

        private void Skip()
        {
            DrawManager.Instance.SkipLevel();

            int nextLevelIndex = curLevelIndex + 1;
            var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
            saveData.GetLevel(curLevelIndex).SkipLevel(false);
            saveData.GetLevel(nextLevelIndex).UnlockLevel(true);
            GameplayInputTransporter.Reset();
            GameplayInputTransporter.LevelIndex = nextLevelIndex;
            SceneLoader.Instance.LoadGameplayScene(stopMusic: false, onCompleted: () => {
               // GameSoundManager.Instance.PlayGameplayBackground(true, 3);
            });

            GameTracking.LogLevelStart(nextLevelIndex);
            GameTracking.LogLevel_Start(nextLevelIndex);
        }

        private void OnHintButtonClicked()
        {
            DrawManager.Instance.CurLevel.ShowHint();
        }

        private void OnShopButtonClicked()
        {
            PopupHUD.Instance.Show<ShopPopup>();
        }

        private void OnIgnoreInput(IgnoreInputEvent param)
        {
        }
    }
}
