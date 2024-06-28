using AtoGame.Base.Helper;
using DG.Tweening;
using System;
using UnityEngine;

namespace AtoGame.Base.UI
{
    public class DOTweenSpriteAlpha : DOTweenTransition
    {
        [SerializeField] private SpriteRenderer target;
        [SerializeField] private bool fromCurrent;
        [SerializeField] private float from;
        [SerializeField] private float to;

        public SpriteRenderer Target { get => target; set => target = value; }
        public bool FromCurrent { get => fromCurrent; set => fromCurrent = value; }
        public float From { get => from; set => from = value; }
        public float To { get => to; set => to = value; }

        private void Reset()
        {
            target = GetComponent<SpriteRenderer>();
        }

        public override void ResetState()
        {
            if (!fromCurrent)
            {
                target.ChangeAlpha(From);
            }
        }

        public override void ToEndState()
        {
            target.ChangeAlpha(To);
        }

        public override void CreateTween(Action onCompleted)
        {
            Tween = target.DOFade(to, Duration);
            base.CreateTween(onCompleted);
        }

#if UNITY_EDITOR

        private float preAlpha;

        public override void Save()
        {
            preAlpha = target.color.a;
        }

        public override void Load()
        {
            target.ChangeAlpha(preAlpha);
        }



        [ContextMenu("Set From")]
        public void SetFromState()
        {
            From = target.color.a;
        }
        [ContextMenu("Set To")]
        public void SetToState()
        {
            to = target.color.a;
        }
        [ContextMenu("Target => From")]
        private void SetStartTarget()
        {
            target.ChangeAlpha(From);
        }
        [ContextMenu("Target => To")]
        private void SetFinishTarget()
        {
            target.ChangeAlpha(To);
        }
#endif
    }
}