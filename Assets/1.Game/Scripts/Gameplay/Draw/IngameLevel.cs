using AtoGame.Base;
using AtoGame.OtherModules.SoundManager;
using System;
using UnityEngine;
using static TrickyBrain.EventKey;

namespace TrickyBrain
{
    public class IngameLevel : MonoBehaviour
    {
        [SerializeField] private Step[] steps;

        private int curStepIndex;
        protected Action onWon;
        protected Action onLosed;

        protected virtual void OnEnable()
        {
            EventDispatcher.Instance.AddListener<IgnoreInputEvent>(OnIgnoreInput);
        }

        protected virtual void OnDisable()
        {
            if(EventDispatcher.Initialized)
            {
                EventDispatcher.Instance.RemoveListener<IgnoreInputEvent>(OnIgnoreInput);
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            if(focus)
            {
                AtoGame.Mediation.AdsEventExecutor.ExecuteInUpdate(() =>
                {
                    GameSoundManager.Instance.StopLoopSFX();
                });
            }
        }

        public virtual void ValidateObject()
        {

        }

        public void InitLevel(Action onWon, Action onLose)
        {
            this.onWon = onWon;
            this.onLosed = onLose;
            foreach(var s in steps)
            {
                s.InitStep();
            }
        }


        public virtual void StartLevel()
        {
            curStepIndex = 0;
            StartCurrentStep();
            EraserTouchController.Instance.Enable = true;
        }

        private void StartCurrentStep()
        {
            steps[curStepIndex].StartStep();
            steps[curStepIndex].SetOnWin(OnStepWin);
            steps[curStepIndex].SetOnLose(OnStepLose);
        }

        private void OnStepWin()
        {
            //steps[curStepIndex].EndStep();

            if(curStepIndex == steps.Length - 1)
            {
                WinLevel();
            }
            else
            {
                curStepIndex++;
                StartCurrentStep();
            }
        }

        public virtual void WinLevel()
        {
            EraserTouchController.Instance.Hide();
            EraserTouchController.Instance.Enable = false;
            onWon?.Invoke();
        }

        private void OnStepLose()
        {
            //steps[curStepIndex].EndStep();
            LoseLevel();
        }

        private void LoseLevel()
        {
            EraserTouchController.Instance.Hide();
            EraserTouchController.Instance.Enable = false;
            onLosed?.Invoke();
        }

        public void ShowHint()
        {
            steps[curStepIndex].PlayHint();
        }

        protected virtual void OnIgnoreInput(IgnoreInputEvent param)
        {
            IgnoreInput(param.EnableIgnore);
        }

        public void IgnoreInput(bool ignoreInput)
        {
            steps[curStepIndex].IgnoreInput(ignoreInput);
        }
    }
}
