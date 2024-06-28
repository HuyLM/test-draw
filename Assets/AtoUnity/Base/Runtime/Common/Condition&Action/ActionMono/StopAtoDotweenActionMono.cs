using AtoGame.Base.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.Base
{
    public class StopAtoDotweenActionMono : ActionMono
    {
        [SerializeField] private DOTweenAnimation anim;
        [SerializeField] private bool completed = false;

        private Action onCompleted;

        public override void Execute(Action onCompleted = null)
        {
            this.onCompleted = onCompleted;
            anim?.Stop(completed);
            OnComplete(this.onCompleted);
        }

        public override void ValidateObject()
        {
            base.ValidateObject();
            if(anim == null)
            {
                Debug.Log($"{name} ValidateObject: anim is null", this);
            }
        }
    }
}