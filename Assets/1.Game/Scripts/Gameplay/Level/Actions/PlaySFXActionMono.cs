using AtoGame.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class PlaySFXActionMono : ActionMono
    {
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private AudioSource audioSource;
        [SerializeField, Range(0, 1)] private float volume = 1f;

        public override void Execute(Action onCompleted = null)
        {
            if(GameSoundManager.Instance.SFXEnable == false)
            { 
                return;
            }
            if(audioSource != null)
            {
                audioSource.clip = audioClip;
                audioSource.loop = true;
                audioSource.volume = volume;
                audioSource.Play();
            }
            else
            {
                GameSoundManager.Instance.PlaySFX(audioClip);
            }
            OnComplete(onCompleted);
        }

        public override void ValidateObject()
        {
            base.ValidateObject();
            if(audioClip == null)
            {
                Debug.Log($"{name} ValidateObject: audioClip null", this);
            }
        }
    }
}
