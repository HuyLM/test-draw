using AtoGame.OtherModules.HUD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TrickyBrain
{
    public class HomePopup : BasePopup
    {
        [SerializeField] private Button btnSetting;
        [SerializeField] private Button btnLevels;
        [SerializeField] private Button btnPressToStart;

        private bool blockPlay;

        protected override void Start()
        {
            base.Start();
            btnSetting.onClick.AddListener(OnSettingButtonClicked);
            btnLevels.onClick.AddListener(OnLevelsButtonClicked);
            btnPressToStart.onClick.AddListener(OnPressToStartButtonClicked);
        }

        protected override void ActiveFrame()
        {
            base.ActiveFrame();
            blockPlay = false;
        }

        private void OnSettingButtonClicked()
        {
            PopupHUD.Instance.Show<SettingPopup>();
        }

        private void OnLevelsButtonClicked()
        {
            GameplayPanel gameplayPanel = PanelHUD.Instance.GetActiveFrame<GameplayPanel>();
            var levelsPopup = PopupHUD.Instance.GetFrame<LevelsPopup>();
            levelsPopup.SetCurrentLevelIndex(gameplayPanel.CurLevelIndex);
            PopupHUD.Instance.Show<LevelsPopup>();
        }

        public void SetBlockPlay(bool block)
        {
            blockPlay = block;
        }

        private void OnPressToStartButtonClicked()
        {
            if(blockPlay)
            {
                return;
            }
            Hide();
            DrawManager.Instance.StartLevel();
        }
    }
}
