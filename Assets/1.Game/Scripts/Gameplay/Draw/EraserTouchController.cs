using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AtoGame.Base;
using AtoGame.OtherModules.LocalSaveLoad;

namespace TrickyBrain
{
    public class EraserTouchController : SingletonBind<EraserTouchController>, ITouchListener
    {
        [SerializeField] Transform goEraser;
        [SerializeField] SpriteRenderer spIcon;

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
            var shopData = LocalSaveLoadManager.Get<ShopSaveData>();
            int id = shopData.UsingItemID;
            var pencilConfig = DataConfigs.Instance.ShopConfigData.GetItem(id);
            if(pencilConfig != null)
            {
                spIcon.sprite = pencilConfig.Icon;
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

        public void Hide()
        {
            goEraser.gameObject.SetActive(false);
        }
    }
}
