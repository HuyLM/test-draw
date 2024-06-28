using AtoGame.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class StopSFXActionMono : ActionMono
    {
        [SerializeField] private AudioSource audioSource;

        public override void Execute(Action onCompleted = null)
        {
            if(audioSource != null)
            {
                audioSource.Stop();
            }
            OnComplete(onCompleted);
        }

        public override void ValidateObject()
        {
            base.ValidateObject();
            if(audioSource == null)
            {
                Debug.Log($"{name} ValidateObject: audioSource null", this);
            }
        }
    }
}
