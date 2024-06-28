using AtoGame.Base;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TrickyBrain
{
    public class DOPathDOTween : ActionMono
    {
        public Transform GameObject;
        public Transform[] arrayPosition;
        public float duration;
        public PathType pathType;
        public PathMode pathMode;
        public Ease ease;
        private Action onComplete = null;

        public override void Execute(Action onCompleted = null)
        {
            Vector3[] arrayVector = arrayPosition.Select(pos => pos.position).ToArray(); 
            this.onComplete = onCompleted;
            GameObject.DOPath(arrayVector, duration, pathType, pathMode).OnComplete(OnComplete).SetEase(ease).SetId(this);
        }
        private void OnComplete()
        {
            OnComplete(onComplete);
        }
        private void OnDisable()
        {
            this.DOKill(true);
        }

        public override void ValidateObject()
        {
            base.ValidateObject();
            if(GameObject == null)
            {
                Debug.Log($"{name} ValidateObject: GameObject null", this);
            }

            if(arrayPosition == null)
            {
                Debug.Log($"{name} ValidateObject: arrayPosition null", this);
            }
        }
    }
}
