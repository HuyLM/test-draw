using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.Base
{
    public class ConditionActionMono : ActionMono
    {
        [SerializeField] private ConditionMono condition;
        [SerializeField] private ActionMono successAction;
        [SerializeField] private ActionMono failAction;

        private Action onCompleted;
        public override void Execute(Action onCompleted = null)
        {
            this.onCompleted = onCompleted;
            if(condition != null)
            {
                if(condition.CheckCondition() == true)
                {
                    if(successAction != null)
                    {
                        successAction.Execute(OnComplete);
                    }
                    else
                    {
                        OnComplete();
                    }
                }
                else
                {
                    if(failAction != null)
                    {
                        failAction.Execute(OnComplete);
                    }
                    else
                    {
                        OnComplete();
                    }
                }
            }
        }

        private void OnComplete()
        {
            OnComplete(onCompleted);
        }

        public override void ValidateObject()
        {
            base.ValidateObject();

        }
    }
}