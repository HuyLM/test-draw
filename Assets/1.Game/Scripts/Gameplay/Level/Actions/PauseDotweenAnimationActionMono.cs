using AtoGame.Base;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class PauseDotweenAnimationActionMono : ActionMono
    {
        [SerializeField] private DOTweenAnimation anim;

        public override void Execute(Action onCompleted = null)
        {
            anim?.DOPause();
            OnComplete(onCompleted);
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
