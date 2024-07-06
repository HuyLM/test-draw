using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TrickyBrain
{
    public class Line : MonoBehaviour
    {
        [SerializeField] private Point[] winPoints;
        [SerializeField] private Point[] losePoints;

        private bool isCompleted;
        private bool isLosed;

        private int curIndex;
        private int startIndex;
        private bool isBlockingInput;

        public bool IsWin => isCompleted == true && isLosed == false;

        public void InitStep()
        {
            isCompleted = false;
            isLosed = false;

            for(int i = 0; i < winPoints.Length; i++)
            {
                winPoints[i].SetLine(this);
                winPoints[i].Hide();
            }

            for(int i = 0; i < losePoints.Length; i++)
            {
                losePoints[i].SetLine(this);
                losePoints[i].Hide();
            }

            curIndex = -1;
            startIndex = -1;
        }

        public void StartStep()
        {
            for(int i = 0; i < winPoints.Length; i++)
            {
                winPoints[i].Show();
            }

            for(int i = 0; i < losePoints.Length; i++)
            {
                losePoints[i].Show();
            }
        }

        public void ResetStep()
        {
            for(int i = 0; i < winPoints.Length; i++)
            {
                winPoints[i].Show();
            }

            for(int i = 0; i < losePoints.Length; i++)
            {
                losePoints[i].Show();
            }

            isCompleted = false;
            isLosed = false;
            curIndex = -1;
            startIndex = -1;
        }

        public bool AddLine(Point point)
        {
#if UNITY_EDITOR
            if(DrawManager.Instance.Drawer.Drawing == false)
            {
                return false;
            }
#endif

            if(isCompleted == true) // if completed but continue draw on other points
            {
                isCompleted = false;
                isLosed = true;
                return true;
            }

            if(startIndex < 0)
            {
                // first point
                startIndex = GetIndexOf(point, winPoints);
                curIndex = startIndex;
                return true;
            }
            else
            {
                // check curIndex 
                int nextIndex = (curIndex + 1) % winPoints.Length;
                int preIndex = curIndex - 1 < 0 ? winPoints.Length - 1 : curIndex - 1;
                if(winPoints[nextIndex] == point)
                {
                    // if match next point
                    curIndex = nextIndex;
                    nextIndex = (curIndex + 1) % winPoints.Length;
                    if(nextIndex == startIndex) // is completed line
                    {
                        isCompleted = true;
                    }
                }
                else if(winPoints[preIndex] == point)
                {
                    // if match previous point
                    curIndex = preIndex;
                    preIndex = curIndex - 1 < 0 ? winPoints.Length - 1 : curIndex - 1;
                    if(preIndex == startIndex) // is completed line
                    {
                        isCompleted = true;
                    }
                }
                else // touch any lose point
                {
                    isCompleted = false;
                    isLosed = true;
                }
                return true;
            }
        }

        private int GetIndexOf(Point point, Point[] points)
        {
            for(int i = 0; i < points.Length; i++)
            {
                if(points[i] == point)
                {
                    return i;
                }
            }
            return -1;
        }

        public void HideAllPoints()
        {
            for(int i = 0; i < winPoints.Length; i++)
            {
                winPoints[i].Hide();
            }

            for(int i = 0; i < losePoints.Length; i++)
            {
                losePoints[i].Hide();
            }
        }

        public void IgnoreInput(bool ignoreInput)
        {
            isBlockingInput = ignoreInput;

            for(int i = 0; i < winPoints.Length; i++)
            {
                winPoints[i].IgnoreInput(ignoreInput);
            }

            for(int i = 0; i < losePoints.Length; i++)
            {
                losePoints[i].IgnoreInput(ignoreInput);
            }
        }
    }
}
