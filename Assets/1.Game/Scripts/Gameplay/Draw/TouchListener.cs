using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TrickyBrain
{
    public interface ITouchListener
    {
        public abstract void OnTouchBegin(TouchListener touch);
        public abstract void OnTouchMoved(TouchListener touch);
        public abstract void OnTouchEnd(TouchListener touch);
    }
    /// <summary>
    /// Listener of touch and mouse general on PC and Mobile
    /// </summary>
    public class TouchListener : MonoBehaviour
    {
        enum TouchState
        {
            TOUCH_NONE, TOUCH_BEGIN, TOUCH_MOVED
        }

        public bool IsOverGameObject = false;
        public ITouchListener ObjectListener;

        public UnityEvent onBeginEvent, onMovedEvent, onEndEvent;

        [HideInInspector]
        public Vector2 beginPosition, position, endPosition;
        TouchState touchState;

        // Start is called before the first frame update
        void Start()
        {
            if(ObjectListener == null)
                ObjectListener = GetComponent<ITouchListener>();
        }

        // Update is called once per frame
        void Update()
        {
            if(ObjectListener == null)
                return;

            if(Input.GetMouseButton(0))
            {
                position = Input.mousePosition;

                if(touchState == TouchState.TOUCH_MOVED)
                {
                    if(ObjectListener != null)
                        ObjectListener.OnTouchMoved(this);
                }
                else if(touchState == TouchState.TOUCH_BEGIN)
                {
                    touchState = TouchState.TOUCH_MOVED;
                    if(ObjectListener != null)
                        ObjectListener.OnTouchMoved(this);
                }
            }

            if(touchState == TouchState.TOUCH_MOVED && Input.GetMouseButtonUp(0))
            {
                endPosition = Input.mousePosition;
                touchState = TouchState.TOUCH_NONE;
                if(ObjectListener != null)
                    ObjectListener.OnTouchEnd(this);
                return;
            }

            if(EventSystem.current == null)
            {
                return;
            }

            if(IsOverGameObject && EventSystem.current.IsPointerOverGameObject())
                return;

            if(Input.GetMouseButtonDown(0))
            {
                beginPosition = Input.mousePosition;
                touchState = TouchState.TOUCH_BEGIN;
                if(ObjectListener != null)
                    ObjectListener.OnTouchBegin(this);
                return;
            }
        }
    }
}
