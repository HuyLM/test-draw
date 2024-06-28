using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace AtoGame.Base.UI
{
    public class DOTweenActive : DOTweenTransition
    {
        [SerializeField] private Transform target;
        [SerializeField] private bool fromCurrent;
        [SerializeField] private bool active;


        public Transform Target { get => target; set => target = value; }
        public bool FromCurrent { get => fromCurrent; set => fromCurrent = value; }
        public bool Active { get => active; set => active = value; }

        private void Reset()
        {
            target = transform;
        }

        public override void ResetState()
        {

            if (!fromCurrent)
            {
                target.gameObject.SetActive(!active);
            }
        }

        public override void ToEndState()
        {
            target.gameObject.SetActive(active);
        }

        public override void CreateTween(Action onCompleted)
        {
            Tween = DOVirtual.DelayedCall(0.01f, () => {
                target.gameObject.SetActive(active);
            });
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                // if get error, must install EditorCoroutines in Package Manager
                Unity.EditorCoroutines.Editor.EditorCoroutineUtility.StartCoroutine(CountEditorUpdates(Delay, () => {
                    target.gameObject.SetActive(active);
                }), this);
            }
#endif
            base.CreateTween(onCompleted);
        }

#if UNITY_EDITOR

        IEnumerator CountEditorUpdates(float delay, Action onCompleted)
        {
            // if get error, must install EditorCoroutines in Package Manager
            var waitForOneSecond = new Unity.EditorCoroutines.Editor.EditorWaitForSeconds(delay);
            yield return waitForOneSecond;
            onCompleted?.Invoke();
        }

        private bool preActive;

        public override void Save()
        {
            preActive = target.gameObject.activeSelf;
        }

        public override void Load()
        {
            target.gameObject.SetActive(preActive);
        }
#endif
    }
}
