using AtoGame.IAP;
using AtoGame.OtherModules.HUD;
using AtoGame.OtherModules.LocalSaveLoad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TrickyBrain
{
    public class HomePanel : BasePanel
    {
        [SerializeField] private Button btnSetting;
        [SerializeField] private Button btnNoAds;
        [SerializeField] private Button btnPlay;
        [SerializeField] private Button btnLevels;

        private void Start()
        {
            btnSetting.onClick.AddListener(OnSettingButtonClicked);
            btnNoAds.onClick.AddListener(OnNoAdsButtonClicked);
            btnPlay.onClick.AddListener(OnPlayButtonClicked);
            btnLevels.onClick.AddListener(OnLevelsButtonClicked);
        }

        protected override void ActiveFrame()
        {
            base.ActiveFrame();

        }


        private void OnSettingButtonClicked()
        {
            PopupHUD.Instance.Show<SettingPopup>();
        }

        private void OnNoAdsButtonClicked()
        {
            AdsSaveData adsSaveData = LocalSaveLoadManager.Get<AdsSaveData>();
            if(adsSaveData.IsRemoveAds == false)
            {
                PopupHUD.Instance.Show<LoadingPopup>();

                IapPurchaseController.Instance.Buy("com.dop.removeads",
                (iapKey) =>
                {
                    PopupHUD.Instance.Hide<LoadingPopup>();
                    GameTracking.LogAdsRemove();
                    AdsManager.Instance.DestroyBanner();
                },
                (fail) =>
                {
                    PopupHUD.Instance.Hide<LoadingPopup>();
                    string message = "key_purchase_iap_failed_message";
                    string title ="key_purchase_iap_failed_title";
                    NoticePopup noticePopup = PopupHUD.Instance.Show<NoticePopup>();
                    noticePopup.SetMessage(message);
                    noticePopup.SetTitle(title);
                });
            }
            else
            {
                string message = "key_bought_purchase_iap_message";
                string title = "key_bought_purchase_iap_title";
                NoticePopup noticePopup = PopupHUD.Instance.Show<NoticePopup>();
                noticePopup.SetMessage(message);
                noticePopup.SetTitle(title);
            }
        }

        private void OnPlayButtonClicked()
        {
            var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
            int levelIndex = saveData.GetCurrentUnlockLevelIndex();
            if (levelIndex < 0)
            {
                levelIndex = saveData.Levels.Count - 1;
            }
            GameplayInputTransporter.Reset();
            GameplayInputTransporter.LevelIndex = levelIndex;
            SceneLoader.Instance.LoadGameplayScene(stopMusic: false, onCompleted: () => {
                GameSoundManager.Instance.PlayGameplayBackground(true, 3);
            });

            GameTracking.LogLevelStart(levelIndex);
            GameTracking.LogLevel_Start(levelIndex);
            /*
            AdsManager.Instance.NativeAd.Destroy(); [Remove-Admob]
            */
        }

        private void OnLevelsButtonClicked()
        {
        }

    }
}
