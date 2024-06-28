using AtoGame.Base.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.Base
{
    public class PlayAtoDotweenActionMono : ActionMono
    {
        [SerializeField] private DOTweenAnimation anim;
        [SerializeField] private bool restart = true;
        [SerializeField] private bool immediateComplete = false;
        [SerializeField, NaughtyAttributes.ShowIf("immediateComplete")] private ActionMono endAction;

        private Action onCompleted;

        public override void Execute(Action onCompleted = null)
        {
            this.onCompleted = onCompleted;
            anim?.Play(()=>{
                if(immediateComplete == true)
                {
                    endAction?.Execute();
                }
                else
                {
                    OnComplete(this.onCompleted);
                }
            }, restart);
            if(immediateComplete == true)
            {
                OnComplete(this.onCompleted);
            }
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