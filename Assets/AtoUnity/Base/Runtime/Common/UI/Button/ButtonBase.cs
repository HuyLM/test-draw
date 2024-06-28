﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AtoGame.Base.UI
{
    public class ButtonBase : Button
    {
        #region Button Base
        [Header("==== Click Scale ====")]
        [SerializeField] protected float clickScale = 0.95f;
        [SerializeField] protected Transform tfScale;
        const float ZoomOutTime = 0.1f;
        const float ZoomInTime = 0.1f;
        Vector3 originScale = new Vector3(1.0f, 1.0f, 1.0f);

        bool invoked = false;
        bool pointerDown = false;

        private Coroutine IStartClick;

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            tfScale = transform;
        }

#endif
        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            if (state == SelectionState.Disabled)
            {
                SetState(false);
            }
            else
            {
                SetState(true);
            }

        }

        protected override void Start()
        {
            base.Start();
            originScale = tfScale.localScale;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ResetInvokeState();
        }

        public void ResetInvokeState()
        {
            invoked = false;
        }

        protected virtual void SetState(bool enable)
        {
        }

        public void AddListener(UnityAction action, bool resetAll = false)
        {
            if (!resetAll)
                onClick.RemoveListener(action);
            else
                onClick.RemoveAllListeners();
            onClick.AddListener(action);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            //if (!EventSystem.current.IsPointerOverGameObject()) return;
            pointerDown = true;
            if (interactable)
            {
                IStartClick = StartCoroutine(StartClick());
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (interactable)
            {
                invoked = true;
                InvokeOnClick();
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (!pointerDown)
                return;

            //if (interactable && (!invokeOnce || !invoked)) {
            //    invoked = true;
            //    InvokeOnClick();
            //}

            pointerDown = false;
            if (IStartClick != null)
            {
                StopCoroutine(IStartClick);
            }
            tfScale.localScale = originScale;
            //		StartCoroutine(StartExit());
        }

        protected virtual void InvokeOnClick()
        {
        }

        IEnumerator StartClick()
        {
            float tCounter = 0;

            while (tCounter < ZoomOutTime)
            {
                tCounter += UnityEngine.Time.deltaTime;
                tfScale.localScale = Vector3.Lerp(originScale, originScale * clickScale, tCounter / ZoomOutTime);
                yield return null;
            }
        }

        IEnumerator StartExit()
        {
            float tCounter = 0;

            while (tCounter < ZoomInTime)
            {
                tCounter += UnityEngine.Time.deltaTime;
                tfScale.localScale = Vector3.Lerp(originScale * clickScale, originScale, tCounter / ZoomInTime);
                yield return null;
            }
        }
        #endregion
    }
}
