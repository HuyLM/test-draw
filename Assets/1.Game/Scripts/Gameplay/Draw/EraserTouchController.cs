using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AtoGame.Base;

namespace TrickyBrain
{
    public class EraserTouchController : SingletonBind<EraserTouchController>, ITouchListener
    {
        [SerializeField] Transform goEraser;

        private Camera _camera;

        public bool Enable { get; set; }

        private void Start()
        {

        }

        public void OnTouchBegin(TouchListener touch)
        {
            if(Enable == false)
            {
                return;
            }
            if(_camera == null)
            {
                _camera = GameplayCamera.Instance.GetCamera();
            }
            goEraser.gameObject.SetActive(true);
            SetEraserPosition(touch.position);
        }

        public void OnTouchEnd(TouchListener touch)
        {
            if(Enable == false)
            {
                return;
            }
            goEraser.gameObject.SetActive(false);
        }

        public void OnTouchMoved(TouchListener touch)
        {
            if(Enable == false)
            {
                return;
            }
            SetEraserPosition(touch.position);
        }

        private void SetEraserPosition(Vector2 positon)
        {
            if(Enable == false)
            {
                return;
            }
            if(_camera == null)
            {
                _camera = GameplayCamera.Instance.GetCamera();
            }
            Vector3 mousePos = positon;
            mousePos.z = _camera.nearClipPlane;
            Vector3 worldPosition = _camera.ScreenToWorldPoint(mousePos);
            goEraser.position = worldPosition;
        }
    }
}
