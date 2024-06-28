using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class Drawer : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private float minDistance = 0.1f;

        private Vector3 previousPosition;
        private bool drawing;
        private bool isBlockingInput;

        public bool Drawing => drawing;
        public Action OnBeginDraw;
        public Action OnEndDraw;

        public void InitStep()
        {
            ResetStep();
        }

        public void StartStep()
        {
            ResetStep();
        }

        public void ResetStep()
        {
            previousPosition = transform.position;
            lineRenderer.positionCount = 0;
            drawing = false;
        }

        void Update()
        {
            if(isBlockingInput == true)
            {
                return;
            }

            if(Input.GetMouseButton(0) || Input.touchCount > 0)
            {
              
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                curPosition.z = 0;

                if(drawing == false)
                {
                    lineRenderer.positionCount = 1;
                    lineRenderer.SetPosition(0, curPosition);
                    drawing = true;
                    OnBeginDraw?.Invoke();
                }
                else
                {
                    if(Vector3.Distance(curPosition, previousPosition) >= minDistance)
                    {
                        lineRenderer.positionCount++;
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, curPosition);
                        previousPosition = curPosition;
                    }
                }
            }
            else
            {
                if(drawing == true)
                {
                    ResetStep();
                    OnEndDraw?.Invoke();
                }
            }
        }

        public void IgnoreInput(bool  ignoreInput)
        {
            isBlockingInput = ignoreInput;
        }    
    }
}
