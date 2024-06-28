using AtoGame.Base;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class AnimatorMonoAction : ActionMono
    {
        [SerializeField]
        private Animation anim;
        [SerializeField]
        private string nameAnim;
        private Action onComplete = null;
        public override void Execute(Action onCompleted = null)
        {
            AnimationClip clip = anim.clip;
            if(!String.IsNullOrEmpty(nameAnim))
            {
                var clipAnim = anim.GetClip(nameAnim);
                if(clipAnim != null)
                {
                    clip = clipAnim;
                }
            }
            anim.clip = clip;
            anim.Play();
            float time = clip.length;
            this.onComplete = onCompleted;
            DOVirtual.DelayedCall(time, () =>
            {
                OnComplete(onComplete);
            });
        }

        public override void ValidateObject()
        {
            base.ValidateObject();
            if(anim == null)
            {
                Debug.Log($"{name} ValidateObject: anim null", this);
            }
        }
    }
}
