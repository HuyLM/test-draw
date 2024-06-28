using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.Base.UI
{
    public class DOTweenAnimation : MonoBehaviour
    {
        public enum DOTweenLoadType { AutoInit, ManualInit, AutoPlay }

        [Header("[Transitions]")]
        [SerializeField] private float delay;
        [SerializeField] private DOTweenTransition[] transitions;
        [SerializeField] private DOTweenLoadType loadType = DOTweenLoadType.AutoInit;

        public float Delay { get => delay; }

        public DOTweenTransition[] Transitions { get => transitions; }

        private Action onCompleted;

        private void Awake()
        {
            if (loadType == DOTweenLoadType.AutoInit || loadType == DOTweenLoadType.AutoPlay)
            {
                Initialize();
            }
        }

        private void OnEnable()
        {
            if (loadType == DOTweenLoadType.AutoPlay)
            {
                Play();
            }
        }

        private void OnDisable()
        {
            if (loadType == DOTweenLoadType.AutoPlay)
            {
                Stop();
            }
        }

        public void LoadTransitions()
        {
            transitions = GetComponentsInChildren<DOTweenTransition>(true);
        }


        public void Initialize()
        {
        }

        public void ResetState()
        {
            foreach (DOTweenTransition transition in transitions)
            {
                transition.Stop();
                transition.ResetState();
            }
        }

        public void Stop(bool onComplete = false)
        {
            foreach (DOTweenTransition transition in transitions)
            {
                transition.Stop(onComplete);
            }
        }

        public void Play()
        {
            Play(null, true);
        }

        public void Play(System.Action onCompleted, bool restart)
        {
            if(delay > 0)
            {
                if(restart)
                {
                    ResetState();
                }
                DOVirtual.DelayedCall(delay, () => {
                    PlayImmediate(onCompleted, restart);
                });
            }
            else
            {
                PlayImmediate(onCompleted, restart);
            }
        }

        public void PlayImmediate(System.Action onCompleted, bool restart)
        {
            Stop(false);
            if (transitions.Length <= 0)
            {
                onCompleted?.Invoke();
            }
            else
            {
                this.onCompleted = onCompleted;
                for (int i = 0; i < transitions.Length; i++)
                {
                    transitions[i].DoTransition(OnTransitionCompleted, restart);
                }
            }
        }

        private void OnCompleted()
        {
            onCompleted?.Invoke();
        }

        private void OnTransitionCompleted()
        {
            for (int i = 0; i < transitions.Length; ++i)
            {
                if (transitions[i].IsCompleted == false)
                {
                    return;
                }
            }
            OnCompleted();
        }
    }
}

