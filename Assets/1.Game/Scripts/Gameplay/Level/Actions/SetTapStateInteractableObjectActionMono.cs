using AtoGame.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class SetTapStateInteractableObjectActionMono : ActionMono
    {
        [SerializeField] private InteractableObject interactableObject;
        [SerializeField] private bool state;

        public override void Execute(Action onCompleted = null)
        {
            if(interactableObject == null)
            {
                Debug.Log($"{gameObject.name} interactableObject is null", this);
            }
            else
            {
                interactableObject.SetTapState(state);
            }
            OnComplete(onCompleted);
        }

        public override void ValidateObject()
        {
            base.ValidateObject();
            if(interactableObject == null)
            {
                Debug.Log($"{name} ValidateObject: interactableObject is null", this);
                return;
            }
        }
    }
}
