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
        [SerializeField] private Button btnBack;
        [SerializeField] private Button btnSkipLevel;
        [SerializeField] private Button btnHint;
        [SerializeField] private DragButton btnLevels;
        [SerializeField] private TextMeshProUGUI txtLevel;
        [SerializeField] private TextMeshProUGUI txtTitle;
        [SerializeField] private UseableObjectCollector useableObjectCollector;
        [SerializeField] private FindableObjectCollector findableObjectCollector;
        [SerializeField] private Timer countdownTimer;
        [SerializeField] private TextMeshProUGUI txtTime;
        [Header("InteractableLevel")]
        [SerializeField] private ScrollRect srUseableObjects;
        [Header("FindoutLevel")]
        [SerializeField] private RectTransform rtAbove12;
        [SerializeField] private RectTransform rtBelow12;

        private int curLevelIndex;

        public int CurLevelIndex => curLevelIndex;

        private void Start()
        {
            btnBack.onClick.AddListener(OnBackButtonClicked);
            btnSkipLevel.onClick.AddListener(OnSkipButtonClicked);
            btnHint.onClick.AddListener(OnHintButtonClicked);
            btnLevels.onClick.AddListener(OnLevelsButtonClicked);
        }
        private void OnEnable()
        {
            EventDispatcher.Instance.AddListener<LoadedLevelEvent>(OnLoadedLevel);
            EventDispatcher.Instance.AddListener<StartLevelEvent>(OnStartLevel);
            EventDispatcher.Instance.AddListener<EndLevelEvent>(OnEndLevel);
            EventDispatcher.Instance.AddListener<IgnoreInputEvent>(OnIgnoreInput);

            EventDispatcher.Instance.AddListener<InitFindableLevelEvent>(InitFindableLevel);
            EventDispatcher.Instance.AddListener<AddFindableObjectsEvent>(OnAddFindableObjects);

            EventDispatcher.Instance.AddListener<InitUseableObjectsEvent>(InitUseableLevel);
            EventDispatcher.Instance.AddListener<AddUseableObjectsEvent>(OnAddUseableObject);
            EventDispatcher.Instance.AddListener<RemoveUseableObjectsEvent>(OnRemoveUseableObjects);
            EventDispatcher.Instance.AddListener<ReplaceUseableObjectEvent>(OnReplaceUseableObject);

            LocalizationHelper.AddOnLocaleChanged(OnLocaleChagned);
        }

        private void OnDisable()
        {
            if(EventDispatcher.Initialized)
            {
                EventDispatcher.Instance.RemoveListener<LoadedLevelEvent>(OnLoadedLevel);
                EventDispatcher.Instance.RemoveListener<StartLevelEvent>(OnStartLevel);
                EventDispatcher.Instance.RemoveListener<EndLevelEvent>(OnEndLevel);
                EventDispatcher.Instance.RemoveListener<IgnoreInputEvent>(OnIgnoreInput);

                EventDispatcher.Instance.RemoveListener<InitFindableLevelEvent>(InitFindableLevel);
                EventDispatcher.Instance.RemoveListener<AddFindableObjectsEvent>(OnAddFindableObjects);

                EventDispatcher.Instance.RemoveListener<InitUseableObjectsEvent>(InitUseableLevel);
                EventDispatcher.Instance.RemoveListener<AddUseableObjectsEvent>(OnAddUseableObject);
                EventDispatcher.Instance.RemoveListener<RemoveUseableObjectsEvent>(OnRemoveUseableObjects);
                EventDispatcher.Instance.RemoveListener<ReplaceUseableObjectEvent>(OnReplaceUseableObject);
            }
            LocalizationHelper.AddOnLocaleChanged(OnLocaleChagned);
        }

        protected override void ActiveFrame()
        {
            base.ActiveFrame();
            btnBack.interactable = false;
            btnHint.interactable = false;

        }

        protected override void OnShowedFrame()
        {
            base.OnShowedFrame();
            btnBack.interactable = true;
            btnHint.interactable = true;

            
        }

        private void OnLoadedLevel(LoadedLevelEvent param)
        {
            SetLevelText(param.LevelIndex);
            SetLevelTitle(param.LevelIndex);
        }

        private void OnStartLevel(StartLevelEvent param)
        {
            countdownTimer.Countdown(param.PlayTime, (elapsed) => {
                TimeSpan timeSpan = TimeSpan.FromSeconds(elapsed);
                txtTime.text = timeSpan.ToString(@"mm\:ss");
            }, () => {
                var gameManager = GameManager.Instance;
                if(gameManager == null)
                {
                    Debug.Log($"{gameObject.name} GameManager is null", this);
                    return;
                }
                var level = gameManager.CurLevel;
                if(level == null)
                {
                    Debug.Log($"{gameObject.name} level is null", this);
                    return;
                }
                level.LoseLevel();
            });
        }

        private void OnEndLevel(EndLevelEvent param)
        {
            countdownTimer.Stop();
        }

        public void SetLevelText(int levelIndex)
        {
            curLevelIndex = levelIndex;
            txtLevel.text = LocalizationHelper.Localize("key_level", new object[] { new { level = levelIndex + 1 } });

            var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
            btnSkipLevel.interactable = saveData.HasNextLevel(curLevelIndex);
        }

        private void SetLevelTitle(int levelIndex)
        {
            var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
            var levelConfig = saveData.GetLevel(levelIndex).Config;

            txtTitle.text = LocalizationHelper.Localize(levelConfig.TitleKey);
        }

        private void OnBackButtonClicked()
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
                    string message = LocalizationHelper.Localize("key_show_ad_failed_message");
                    string title = LocalizationHelper.Localize("key_show_ad_failed_title");
                    NoticePopup noticePopup = PopupHUD.Instance.Show<NoticePopup>();
                    noticePopup.SetMessage(message);
                    noticePopup.SetTitle(title);
                });
            }
            else
            {
                GameTracking.LogShowAds(false, "feature_skip");
                string message = LocalizationHelper.Localize("key_ad_not_availiable_message");
                string title = LocalizationHelper.Localize("key_ad_not_availiable_title");
                NoticePopup noticePopup = PopupHUD.Instance.Show<NoticePopup>();
                noticePopup.SetMessage(message);
                noticePopup.SetTitle(title);
            }
        }

        private void Skip()
        {
            GameManager.Instance.SkipLevel();

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
            countdownTimer.TogglePause();
            PopupHUD.Instance.GetFrame<HintPopup>().SetLevelIndex(curLevelIndex);
            PopupHUD.Instance.Show<HintPopup>().OnClose(()=> {
                countdownTimer.TogglePause();
            });
        }

        private void OnLevelsButtonClicked()
        {
            var levelsPopup = PopupHUD.Instance.GetFrame<LevelsPopup>();
            levelsPopup.SetCurrentLevelIndex(curLevelIndex);
            PopupHUD.Instance.Show<LevelsPopup>();
        }

        private void OnIgnoreInput(IgnoreInputEvent param)
        {
            if(useableObjectCollector.gameObject.activeInHierarchy)
            {
                useableObjectCollector.IgnoreInput(param.EnableIgnore);
            }
            //HUDManager.Instance.IgnoreUserInput(param.EnableIgnore);
        }

        private void InitUseableLevel(InitUseableObjectsEvent param)
        {
            findableObjectCollector.gameObject.SetActive(false);
            useableObjectCollector.gameObject.SetActive(true);
            useableObjectCollector.SetCapacity(0);
            useableObjectCollector.SetItems(new List<UseableObject>());
            useableObjectCollector.Show();
            useableObjectCollector.Add(param.UseableObjects);
            srUseableObjects.horizontalNormalizedPosition = 0;
            useableObjectCollector.IgnoreInput(false);
            useableObjectCollector.Init();
        }

        private void OnAddUseableObject(AddUseableObjectsEvent param)
        {
            useableObjectCollector.Add(new UseableObject[] { param.UseableObject });
          
            countdownTimer.TogglePause();
            var claimItemPopup = PopupHUD.Instance.Show<ClaimItemPopup>();
            claimItemPopup.SetIcon(param.UseableObject.Sprite);
            claimItemPopup.SetOnConfirm(()=> {
                srUseableObjects.horizontalNormalizedPosition = 1;
                countdownTimer.TogglePause();
            });
        }

        private void OnRemoveUseableObjects(RemoveUseableObjectsEvent param)
        {
            useableObjectCollector.Remove(param.UseableObjects);
        }

        private void OnReplaceUseableObject(ReplaceUseableObjectEvent param)
        {
            useableObjectCollector.Replace(param.From, param.To);

            countdownTimer.TogglePause();
            var claimItemPopup = PopupHUD.Instance.Show<ClaimItemPopup>();
            claimItemPopup.SetIcon(param.To.Sprite);
            claimItemPopup.SetOnConfirm(() => {
                srUseableObjects.horizontalNormalizedPosition = 1;
                countdownTimer.TogglePause();
            });
        }

        private void InitFindableLevel(InitFindableLevelEvent param)
        {
            findableObjectCollector.gameObject.SetActive(true);
            if(param.NeedFindableObjectNumber > 12)
            {
                UnityHelper.CopyRectTransform(rtAbove12, findableObjectCollector.RectTransform());
            }
            else
            {
                UnityHelper.CopyRectTransform(rtBelow12, findableObjectCollector.RectTransform());
            }
            useableObjectCollector.gameObject.SetActive(false);
            FindableObject[] findableObjects = new FindableObject[param.NeedFindableObjectNumber];
            findableObjectCollector.SetCapacity(param.NeedFindableObjectNumber).SetItems(findableObjects).Show();
        }


        private void OnAddFindableObjects(AddFindableObjectsEvent param)
        {
            findableObjectCollector.Add(param.FindableObjects);
            GameSoundManager.Instance.PlayCorrect();
        }

        private void OnLocaleChagned(Locale locale)
        {
            SetLevelText(curLevelIndex);
            SetLevelTitle(curLevelIndex);
        }
    }
}
