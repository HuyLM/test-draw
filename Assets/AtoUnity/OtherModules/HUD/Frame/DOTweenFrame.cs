using AtoGame.Base.UI;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.HUD
{
    public class DOTweenFrame : Frame
    {


        [Foldout("Frame Animations")]
        [SerializeField] protected DOTweenAnimation showAnimation;
        [Foldout("Frame Animations")]
        [SerializeField] protected DOTweenAnimation hideAnimation;
        [Foldout("Frame Animations")]
        [SerializeField] protected DOTweenAnimation pauseAnimation;
        [Foldout("Frame Animations")]
        [SerializeField] protected DOTweenAnimation resumeAnimation;

        protected override void OnInitialize(HUD hud)
        {
            base.OnInitialize(hud);
            InitializeAnimation();
        }

        private void InitializeAnimation()
        {
            showAnimation?.Initialize();
            hideAnimation?.Initialize();
            pauseAnimation?.Initialize();
            resumeAnimation?.Initialize();
        }

        protected override void ActiveFrame()
        {
            hideAnimation?.Stop();
            pauseAnimation?.Stop();
            if(instant || showAnimation == null)
            {
                base.ActiveFrame();
            }
            else
            {
                gameObject.SetActive(true);
                showAnimation.Play(OnShowedFrame, true);
            }
        }

        protected override void DeactiveFrame()
        {
            if (instant || hideAnimation == null)
            {
                base.DeactiveFrame();
            }
            else
            {
                hideAnimation.Play(OnHiddenFrame, true);
            }
        }

        protected override void ResumeFrame()
        {
            if (instant || resumeAnimation == null)
            {
                base.ResumeFrame();
            }
            else
            {
                resumeAnimation.Play(OnResumedFrame, true);
            }
        }

        protected override void PauseFrame()
        {
            if (instant || pauseAnimation == null)
            {
                base.PauseFrame();
            }
            else
            {
                pauseAnimation.Play(OnPausedFrame, true);
            }
        }

    }
}
