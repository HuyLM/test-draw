using AtoGame.OtherModules.HUD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TrickyBrain
{
    public class ClaimItemPopup : NoticePopup
    {
        [SerializeField] private Image imgIcon;

        protected override void ActiveFrame()
        {
            base.ActiveFrame();
            GameSoundManager.Instance.PlayClaim();
        }

        public void SetIcon(Sprite icon)
        {
            imgIcon.sprite = icon;
        }
    }
}
