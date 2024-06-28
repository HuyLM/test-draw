using AtoGame.Base.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TrickyBrain
{
    public class UseableObjectDisplayer : Displayer<UseableObject>, IPointerDownHandler, IDragHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField] private Image imgIcon;
        [SerializeField] private Image imgBG;
        [SerializeField] private UseableDrager drager;

        private bool canDrag = true;

        private Camera _camera;

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
        
        public override void Show()
        {
            if(Model == null)
            {
                return;
            }

            imgIcon.sprite = Model.Sprite;

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(canDrag == false)
            {
                return;
            }
            drager.transform.position = transform.position;
            drager.SetModel(Model).Show();
            drager.gameObject.SetActive(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(canDrag == false)
            {
                return;
            }
            drager.transform.position = transform.position;
            drager.gameObject.SetActive(false);
            Vector3 touchPosition = Camera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 0));
            var collider = Physics2D.OverlapPoint(touchPosition);
            if(collider != null)
            {
                var hitbox = collider.GetComponent<InteracableHitbox>();
                if(hitbox != null && hitbox.InteractableObject != null)
                {
                    hitbox.InteractableObject.InteractWithUseableObject(Model);
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(canDrag == false)
            {
                return;
            }
            Vector3 curPosition = Camera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 10));
            drager.transform.position = curPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //var scrollRect = GetComponentInParent<ScrollRect>();
            //eventData.pointerDrag = scrollRect.gameObject;
            //EventSystem.current.SetSelectedGameObject(scrollRect.gameObject);

            //scrollRect.OnInitializePotentialDrag(eventData);
            //scrollRect.OnBeginDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
           
        }

        public void IgnoreInput(bool ignoreInput)
        {
            canDrag = ignoreInput == false;
            imgBG.raycastTarget = canDrag;
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject dropped = eventData.pointerDrag;
            var useableObjectDisplayer = dropped.GetComponent<UseableObjectDisplayer>();
            if(useableObjectDisplayer != null)
            {
                Model.InteractWithUseableObject(useableObjectDisplayer.Model);
            }
        }
    }
}
