using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

namespace TrickyBrain
{
    public class DragButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] private RectTransform rt;
        [SerializeField] private RectTransform rtMaxY;
        [SerializeField] private RectTransform rtMinY;
        [SerializeField] private RectTransform rtLeft;
        [SerializeField] private RectTransform rtRight;
        public ButtonClickedEvent onClick;
        private Camera _camera;
        private bool isDraging;


        public Camera Camera
        {
            get
            {
                if(_camera == null)
                {
                    _camera = GameplayCamera.Instance.GetCamera();
                }
                return _camera;
            }
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            isDraging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 curPosition = Camera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 10));
            transform.position = curPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDraging = false;
            Vector2 position;
            Vector3 curPosition = Camera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 10));

            if(curPosition.x <= 0)
            {
                rt.anchorMin = new Vector2(0, 0.5f);
                rt.anchorMax = new Vector2(0, 0.5f);
                position.x = rtLeft.transform.position.x;
            }
            else
            {
                rt.anchorMin = new Vector2(1, 0.5f);
                rt.anchorMax = new Vector2(1, 0.5f);
                position.x = rtRight.transform.position.x;
            }    
            position.y = Mathf.Clamp(rt.transform.position.y, rtMinY.transform.position.y, rtMaxY.transform.position.y);
            rt.transform.position = position;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(isDraging)
            {
                return;
            }
            onClick?.Invoke();
        }
    }
}
