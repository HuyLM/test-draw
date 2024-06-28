using AtoGame.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TrickyBrain.EventKey;

namespace TrickyBrain
{
    public abstract class BaseLevel : MonoBehaviour
    {
        [SerializeField] private float playTime = 120; // seconds

        protected Action onWon;
        protected Action onLosed;
        protected bool isEndLevel;

        public float PlayTime => playTime;

        protected virtual void OnEnable()
        {
            EventDispatcher.Instance.AddListener<IgnoreInputEvent>(OnIgnoreInput);
        }

        protected virtual void OnDisable()
        {
            if(EventDispatcher.Initialized)
            {
                EventDispatcher.Instance.RemoveListener<IgnoreInputEvent>(OnIgnoreInput);
            }
        }

        public virtual void ValidateObject()
        {

        }

        public virtual void InitLevel(Action onWon, Action onLose)
        {
            this.onWon = onWon;
            this.onLosed = onLose;
        }

        public virtual void StartLevel()
        {
            isEndLevel = false;
        }

        protected virtual void EndLevel()
        {
            EventDispatcher.Instance.Dispatch(new IgnoreInputEvent() { EnableIgnore = true });
        }

        public virtual void WinLevel()
        {
            if(isEndLevel == true)
            {
                return;
            }
            isEndLevel = true;
            EndLevel();
            onWon?.Invoke();
        }

        public virtual void LoseLevel()
        {
            if(isEndLevel == true)
            {
                return;
            }
            isEndLevel = true;
            EndLevel();
            onLosed?.Invoke();
        }

        protected virtual void OnIgnoreInput(IgnoreInputEvent param)
        {
           
        }
    }
}
