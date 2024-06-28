using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.DemiLib;
using AtoGame.Base;
using static TrickyBrain.EventKey;

namespace TrickyBrain
{
    public class CameraTexture : MonoBehaviour
    {
        public float delay;
        public float duration;
        public Ease ease;

        private void OnEnable()
        {
            EventDispatcher.Instance.AddListener<MoveCameraTo>(MoveToPosition);
        }

        private void OnDisable()
        {
            if(EventDispatcher.Initialized)
            {
                EventDispatcher.Instance.RemoveListener<MoveCameraTo>(MoveToPosition);
            }
        }

        public void MoveToPosition(MoveCameraTo param)
        {
            Vector3 endPosition = new Vector3(param.Position.x, param.Position.y, transform.position.z);
            Tween tween = transform.DOMove(endPosition, duration).SetEase(ease);
            if(delay > 0)
            {
                tween.SetDelay(delay);
            }
        }
    }
}
