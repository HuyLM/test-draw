using AtoGame.Base;
using AtoGame.Base.Utilities;
using AtoGame.OtherModules.HUD;
using AtoGame.OtherModules.LocalSaveLoad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TrickyBrain.EventKey;

namespace TrickyBrain
{
    public class WinPopup : BasePopup
    {
        [SerializeField] private Button btnHome;
        [SerializeField] private Button btnReplay;
        [SerializeField] private Button btnNext;
        [SerializeField] private Button btnSkip;
        [SerializeField] private Image imgLevel;
        [SerializeField] private GameObject goWin;
        [SerializeField] private GameObject goLose;
        private int _curLevelIndex;
        private Vector2 highlightPosition;
        private bool isWin;

        protected override void Start()
        {
            base.Start();
            btnHome.onClick.AddListener(OnHomeButtonClicked);
            btnReplay.onClick.AddListener(OnReplayButtonClicked);
            btnNext.onClick.AddListener(OnNextButtonClicked);
            btnSkip.onClick.AddListener(OnSkipButtonClicked);
        }

        protected override void ActiveFrame()
        {
            base.ActiveFrame();
            var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
            LevelConfig levelConfig = saveData.GetLevel(_curLevelIndex).Config;
            imgLevel.sprite = levelConfig.Image;

            EventDispatcher.Instance.Dispatch(new MoveCameraTo() { 
                Position = highlightPosition
            });
            if(isWin == true)
            {
                bool canNextLevel = false;
                int nextLevelIndex = saveData.GetCurrentUnlockLevelIndex();
                bool hasUnlockLevel = nextLevelIndex > 0;
                if(hasUnlockLevel == true)
                {
                    canNextLevel = true;
                }
                else
                {

                }
                canNextLevel = saveData.HasNextLevel(_curLevelIndex);
                btnNext.SetStateButton(canNextLevel, true);
                btnSkip.SetStateButton(false, false);
            }
            else
            {
                bool hasNextLevel = saveData.HasNextLevel(_curLevelIndex);
                btnSkip.SetStateButton(hasNextLevel, true);
                btnNext.SetStateButton(false, false);
            }
        }

        public void SetWinLevelIndex(int levelIndex)
        {
            _curLevelIndex = levelIndex;
        }
        public void SetHighlightPosition(Vector2 position)
        {
            this.highlightPosition = position;
        }

        public void SetWin(bool isWin)
        {
            this.isWin = isWin;
            goWin.SetActive(isWin);
            goLose.SetActive(isWin == false);
        }

        private void OnHomeButtonClicked()
        {
            if(AdsManager.Instance.IsInterstitialReady())
            {
                AdsManager.Instance.ShowInterstitial("GotoHome", GotoHome, GotoHome);
            }
            else
            {
                GotoHome();
            }
        }

        private void GotoHome()
        {
            Hide();
            PopupHUD.Instance.Show<HomePopup>().SetBlockPlay(true);
            GameManager.Instance.Spawn(_curLevelIndex, () => {
                // GameManager.Instance.StartLevel();
                PopupHUD.Instance.GetActiveFrame<HomePopup>().SetBlockPlay(false);
            });
                        
        }

        private void OnReplayButtonClicked()
        {
            if(AdsManager.Instance.IsInterstitialReady())
            {
                AdsManager.Instance.ShowInterstitial("Replay", Replay, Replay);
            }
            else
            {
                Replay();
            }
        }

        private void Replay()
        {
            Hide();
            PopupHUD.Instance.Show<HomePopup>().SetBlockPlay(true);
            GameTracking.LogReplayLevel(_curLevelIndex);
            GameManager.Instance.Spawn(_curLevelIndex, () => {
                GameManager.Instance.StartLevel();
                PopupHUD.Instance.Hide<HomePopup>();
            });
           
        }

        private void OnNextButtonClicked()
        {
            AppSeasonData.NextLevelCount++;
            int showAdsThreshold = DataConfigs.Instance.IngameConfigData.GetShowInterstitialThreshold(_curLevelIndex);
            if(AppSeasonData.NextLevelCount >= showAdsThreshold)
            {
                if(AdsManager.Instance.IsInterstitialReady())
                {
                    AdsManager.Instance.ShowInterstitial("Next", ()=> {
                        AppSeasonData.NextLevelCount = 0;
                        NextLevel();
                    }, ()=> {
                        NextLevel();
                    });
                }
                else
                {
                    NextLevel();
                }
            }
            else
            {
                NextLevel();
            }
        }

        private void NextLevel()
        {
            var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
            int nextLevelIndex = saveData.GetCurrentUnlockLevelIndex();
            if(nextLevelIndex < 0)
            {
                nextLevelIndex = _curLevelIndex + 1;
            }
            Hide();
            PopupHUD.Instance.Show<HomePopup>().SetBlockPlay(true);
            GameManager.Instance.Spawn(nextLevelIndex, () => {
                GameManager.Instance.StartLevel();
                PopupHUD.Instance.Hide<HomePopup>();
            });
        }

        private void OnSkipButtonClicked()
        {
            if(AdsManager.Instance.IsRewardVideoReady())
            {
                GameTracking.LogShowAds(true, "feature_skip_lose");
                AdsManager.Instance.ShowRewardVideo("Ingame_Skip_Lose", () => {
                    SkipLevel();
                }, ()=> {
                    GameTracking.LogShowAds(false, "feature_skip_lose");
                    string message = LocalizationHelper.Localize("key_show_ad_failed_message");
                    string title = LocalizationHelper.Localize("key_show_ad_failed_title");
                    NoticePopup noticePopup = PopupHUD.Instance.Show<NoticePopup>();
                    noticePopup.SetMessage(message);
                    noticePopup.SetTitle(title);
                });
            }
            else
            {
                GameTracking.LogShowAds(false, "feature_skip_lose");
                string message = LocalizationHelper.Localize("key_ad_not_availiable_message");
                string title = LocalizationHelper.Localize("key_ad_not_availiable_title");
                NoticePopup noticePopup = PopupHUD.Instance.Show<NoticePopup>();
                noticePopup.SetMessage(message);
                noticePopup.SetTitle(title);
            }
        }

        private void SkipLevel()
        {
            GameManager.Instance.SkipLevel();

            int nextLevelIndex = _curLevelIndex + 1;
            var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
            saveData.GetLevel(_curLevelIndex).SkipLevel(false);
            saveData.GetLevel(nextLevelIndex).UnlockLevel(true);
            Hide();
            PopupHUD.Instance.Show<HomePopup>().SetBlockPlay(true);
            GameManager.Instance.Spawn(nextLevelIndex, () => {
                GameManager.Instance.StartLevel();
                PopupHUD.Instance.Hide<HomePopup>();
            });
        }
    }
}
