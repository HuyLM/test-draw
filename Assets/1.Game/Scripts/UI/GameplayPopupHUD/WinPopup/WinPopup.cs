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
        [SerializeField] private Button btnReplay;
        [SerializeField] private Button btnNext;
        [SerializeField] private Image imgLevel;
        private int _curLevelIndex;
        private Vector2 highlightPosition;
        private bool isWin;

        protected override void Start()
        {
            base.Start();
            btnReplay.onClick.AddListener(OnReplayButtonClicked);
            btnNext.onClick.AddListener(OnNextButtonClicked);
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
            }
            else
            {
                bool hasNextLevel = saveData.HasNextLevel(_curLevelIndex);
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
            GameTracking.LogReplayLevel(_curLevelIndex);
            DrawManager.Instance.SpawnLevel(_curLevelIndex, () => {
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
            DrawManager.Instance.SpawnLevel(nextLevelIndex, () => {
            });
        }
    }
}
