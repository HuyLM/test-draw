using AtoGame.OtherModules.HUD;
using AtoGame.OtherModules.LocalSaveLoad;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TrickyBrain
{
    public class HintPopup : BasePopup
    {
        [SerializeField] private Button btnHint;
        [SerializeField] private Button btnAnswer;

        [SerializeField] private Image imgHint;
        [SerializeField] private Image imgAnswer;

        [SerializeField] private TextMeshProUGUI txtHint;
        [SerializeField] private TextMeshProUGUI txtAnswer;

        private int levelIndex;
        private LevelConfig levelConfig;

        protected override void Start()
        {
            base.Start();
            btnHint.onClick.AddListener(OnHintButtonClicked);
            btnAnswer.onClick.AddListener(OnAnswerButtonClicked);
        }
        
        public void SetLevelIndex(int levelIndex)
        {
            var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
            this.levelIndex = levelIndex;
            levelConfig = saveData.GetLevel(levelIndex).Config;
        }


        protected override void ActiveFrame()
        {
            base.ActiveFrame();
            ShowHint();
        }

        private void ShowHint()
        {
           
        }


        private void OnHintButtonClicked()
        {
            if(AdsManager.Instance.IsRewardVideoReady())
            {
                GameTracking.LogShowAds(true, "feature_hint");
                AdsManager.Instance.ShowRewardVideo("Ingame_Hint", () => {
                    GameManager.Instance.UseHint();
                    ShowHint();
                }, () => {
                    GameTracking.LogShowAds(false, "feature_hint");
                    string message = LocalizationHelper.Localize("key_show_ad_failed_message");
                    string title = LocalizationHelper.Localize("key_show_ad_failed_title");
                    NoticePopup noticePopup = PopupHUD.Instance.Show<NoticePopup>();
                    noticePopup.SetMessage(message);
                    noticePopup.SetTitle(title);
                });
            }
            else
            {
                GameTracking.LogShowAds(false, "feature_hint");
                string message = LocalizationHelper.Localize("key_ad_not_availiable_message");
                string title = LocalizationHelper.Localize("key_ad_not_availiable_title");
                NoticePopup noticePopup = PopupHUD.Instance.Show<NoticePopup>();
                noticePopup.SetMessage(message);
                noticePopup.SetTitle(title);
            }
        }

        private void OnAnswerButtonClicked()
        {
            if(AdsManager.Instance.IsRewardVideoReady())
            {
                GameTracking.LogShowAds(true, "feature_answer");
                AdsManager.Instance.ShowRewardVideo("Ingame_Answer", () => {
                    GameManager.Instance.UseAnswer();
                    ShowHint();
                }, () => {
                    GameTracking.LogShowAds(false, "feature_answer");
                    string message = LocalizationHelper.Localize("key_show_ad_failed_message");
                    string title = LocalizationHelper.Localize("key_show_ad_failed_title");
                    NoticePopup noticePopup = PopupHUD.Instance.Show<NoticePopup>();
                    noticePopup.SetMessage(message);
                    noticePopup.SetTitle(title);
                });
            }
            else
            {
                GameTracking.LogShowAds(false, "feature_answer");
                string message = LocalizationHelper.Localize("key_ad_not_availiable_message");
                string title = LocalizationHelper.Localize("key_ad_not_availiable_title");
                NoticePopup noticePopup = PopupHUD.Instance.Show<NoticePopup>();
                noticePopup.SetMessage(message);
                noticePopup.SetTitle(title);
            }
         
        }
    }

    public enum HintState
    {
        Ready, Using, Pausing
    }
}
