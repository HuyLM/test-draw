using AtoGame.Base.UI;
using AtoGame.OtherModules.HUD;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TrickyBrain
{
    public class ToastPopup : BasePopup
    {
        [SerializeField] private DOTweenAnimation daShowToast;
        [SerializeField] private DOTweenAnimation daHideToast;

        protected override void OnInitialize(HUD hud)
        {
            base.OnInitialize(hud);
            daShowToast?.Initialize();
            daHideToast?.Initialize();
        }

        public void ShowToast(string message)
        {
            messageText.text = message;
            daHideToast?.Stop();
            daShowToast?.Play(()=> {
                daHideToast?.Play(()=> {
                    Hide();
                }, true);
            }, true);
        }

    }
}
