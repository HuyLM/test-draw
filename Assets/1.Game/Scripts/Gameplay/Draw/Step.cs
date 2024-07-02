using AtoGame.Base;
using AtoGame.OtherModules.SoundManager;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public enum HintState
    {
        Ready, Using, Pausing
    }

    public class Step : MonoBehaviour
    {
        [Space]
        [BoxGroup("Start Action")]
        [SerializeField] private ActionMono startStepAction;
        [BoxGroup("Win")]
        [SerializeField] private ActionMono winStepAction;
        [Space]
        [BoxGroup("Restart Action")]
        [SerializeField] private ActionMono restartStepAction;
        [Space]
        [BoxGroup("Lose")]
        [SerializeField] private ActionMono loseAction;
        [Space]
        [BoxGroup("Hint Actions")]
        [SerializeField] private ActionMono playHintStepAction;
        [BoxGroup("Hint Actions")]
        [SerializeField] private ActionMono stopHintStepAction;


        [SerializeField] private Drawer drawer;
        [SerializeField] private Line line;

        protected Action onWon;
        protected Action onLosed;
        protected bool isEndStep;
        private HintState hintState;


        public virtual void ValidateObject()
        {

        }
        public void InitStep()
        {
            drawer.OnBeginDraw = OnBeginDraw;
            drawer.OnEndDraw = OnEndDraw;
            drawer.InitStep();
            line.InitStep();
            IgnoreInput(true);
        }

        private void OnBeginDraw()
        {
            GameSoundManager.Instance.PlayDefaultEraser();
            if(hintState == HintState.Using)
            {
                StopHint();
            }
        }

        private void OnEndDraw()
        {
            GameSoundManager.Instance.StopLoopSFX();
            if(line.IsWin == true)
            {
                DoActionsAfterWin();
            }
            else // not complete
            {
                ResetStep();
            }
        }

        public void StartStep()
        {
            drawer.StartStep();
            line.StartStep();
            IgnoreInput(false);
        }

        private void ResetStep()
        {
            drawer.ResetStep();
            line.ResetStep();
            restartStepAction?.Execute();
            if(hintState == HintState.Pausing)
            {
                PlayHint();
            }
        }

        private void DoActionsAfterWin()
        {
            if(isEndStep == true)
            {
                return;
            }
            isEndStep = true;
            IgnoreInput(true);

            if(winStepAction != null)
            {
                winStepAction.Execute(onWon);
            }
            else
            {
                onWon?.Invoke();
            }
        }

        private void DoActionsAfterLose()
        {
            if(isEndStep == true)
            {
                return;
            }
            isEndStep = true;
            IgnoreInput(true);

            if(loseAction != null)
            {
                loseAction.Execute(onLosed);
            }
            else
            {
                onLosed?.Invoke();
            }
        }

        [NaughtyAttributes.Button]
        public void PlayHint()
        {
            hintState = HintState.Using;
            playHintStepAction?.Execute();
        }

        private void StopHint()
        {
            hintState = HintState.Pausing;
            stopHintStepAction?.Execute();
        }

        public void SetOnWin(Action onWin)
        {
            this.onWon = onWin;
        }

        public void SetOnLose(Action onLose)
        {
            this.onLosed = onLose;
        }

        public void IgnoreInput(bool ignoreInput)
        {
            drawer.IgnoreInput(ignoreInput);
            line.IgnoreInput(ignoreInput);
        }
    }
}
