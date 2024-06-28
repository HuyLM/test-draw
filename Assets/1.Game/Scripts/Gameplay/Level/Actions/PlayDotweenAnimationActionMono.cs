using AtoGame.Base;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class PlayDotweenAnimationActionMono : ActionMono
    {
        [SerializeField] private DOTweenAnimation startAnim;
        [SerializeField] private DOTweenAnimation endAnim;

        private Action onCompleted;
        public override void Execute(Action onCompleted = null)
        {
            this.onCompleted = onCompleted;
            startAnim?.DOGotoAndPlay(0);
            if(endAnim != null)
            {
                if(endAnim.onComplete == null)
                {
                    endAnim.onComplete = new UnityEngine.Events.UnityEvent();
                }
                endAnim.onComplete.AddListener(OnComplete);
            }
            else
            {
                OnComplete(this.onCompleted);
            }
        }

        private void OnComplete()
        {
            OnComplete(onCompleted);
        }

        public override void ValidateObject()
        {
            base.ValidateObject();
            if(startAnim == null)
            {
                Debug.Log($"{name} ValidateObject: startAnim null", this);
            }
        }
    }
}
