using AtoGame.OtherModules.HUD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using AtoGame.OtherModules.LocalSaveLoad;

namespace TrickyBrain
{
    public class LevelsPopup : BasePopup
    {
        [SerializeField] private TextMeshProUGUI txtPage;
        [SerializeField] private Button btnNext;
        [SerializeField] private Button btnPrevious;
        [SerializeField] private LevelCollector levelCollector;
        [SerializeField] private int numberLevelInPage = 9;

        private int curPage;
        private int originPage;
        private int maxPage;
        int curLevelIndex;

        protected override void Start()
        {
            base.Start();
            btnNext.onClick.AddListener(OnNextButtonClicked);
            btnPrevious.onClick.AddListener(OnPreviousButtonClicked);
        }

        protected override void ActiveFrame()
        {
            base.ActiveFrame();

            // get cur page
            var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
            int levelCount = saveData.Levels.Count;
            maxPage = (levelCount - 1) / numberLevelInPage + 1;
            curPage = curLevelIndex / numberLevelInPage + 1;
            originPage = curPage;
            ShowPage();
        }

        public void SetCurrentLevelIndex(int levelIndex)
        {
            curLevelIndex = levelIndex;
        }

        private void ShowPage()
        {
            txtPage.text = $"{curPage}/{maxPage}";
            if(curPage == 1)
            {
                btnPrevious.interactable = false;
            }
            else
            {
                btnPrevious.interactable = true;
            }
            if(curPage == maxPage)
            {
                btnNext.interactable = false;
            }
            else
            {
                btnNext.interactable = true;
            }

            var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
            int pageIndex = curPage - 1;
            int index = pageIndex * numberLevelInPage;
            int count = saveData.Levels.Count - index < numberLevelInPage ? saveData.Levels.Count - index : numberLevelInPage;
            List<LevelData> leves = saveData.Levels.GetRange(index, count);
            levelCollector.SetCurrentLevelIndex(curLevelIndex);
            levelCollector.SetOnSelected(OnSelectedLevel).SetItems(leves).SetCapacity(leves.Count).Show();
        }

        private void OnSelectedLevel(LevelDisplayer displayer)
        {
            if(displayer.Model.State == LevelState.Locked)
            {
                var popup = PopupHUD.Instance.Show<ConfirmUnlockLevelPopup>();
                popup.SetOnConfirm(()=> {
                    if(AdsManager.Instance.IsRewardVideoReady())
                    {
                        GameTracking.LogShowAds(true, "unlock_level");
                        AdsManager.Instance.ShowRewardVideo("Unlock_Level", () => {
                            Hide();
                            var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
                            saveData.UnlockLevel(displayer.Model.Index);
                            SpawnLevel(displayer.Model.Index);
                        }, () => {
                            GameTracking.LogShowAds(false, "unlock_level");
                            string message = LocalizationHelper.Localize("key_show_ad_failed_message");
                            string title = LocalizationHelper.Localize("key_show_ad_failed_title");
                            NoticePopup noticePopup = PopupHUD.Instance.Show<NoticePopup>();
                            noticePopup.SetMessage(message);
                            noticePopup.SetTitle(title);
                        });
                    }
                    else
                    {
                        GameTracking.LogShowAds(false, "unlock_level");
                        string message = LocalizationHelper.Localize("key_ad_not_availiable_message");
                        string title = LocalizationHelper.Localize("key_ad_not_availiable_title");
                        NoticePopup noticePopup = PopupHUD.Instance.Show<NoticePopup>();
                        noticePopup.SetMessage(message);
                        noticePopup.SetTitle(title);
                    }
                });
            }
            else if (displayer.Model.State == LevelState.Unlocked)
            {
                Hide();
                SpawnLevel(displayer.Model.Index);
            }
            else if(displayer.Model.State == LevelState.Completed)
            {
                Hide();
                SpawnLevel(displayer.Model.Index);
            }
        }

        private void SpawnLevel(int index)
        {
            GameManager.Instance.Spawn(index, () => {
                // GameManager.Instance.StartLevel();
                PopupHUD.Instance.Show<HomePopup>(); 
            });
        }

        private void OnNextButtonClicked()
        {
            curPage++;
            ShowPage();
        }

        private void OnPreviousButtonClicked()
        {
            curPage--;
            ShowPage();
        }
    }
}
