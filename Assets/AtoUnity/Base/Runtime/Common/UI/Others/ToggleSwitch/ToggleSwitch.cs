using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AtoGame.Base.UI
{
    public class ToggleSwitch : MonoBehaviour
    {
        [SerializeField] DOTweenAnimation daOn;
        [SerializeField] DOTweenAnimation daOff;
        [SerializeField] Button btn;

        protected bool _curState;
        public Action<bool> OnChangeSwitch;

        protected void Awake()
        {
            daOff?.Initialize();
            daOn?.Initialize();
            btn.onClick.AddListener(OnButtonClicked);
        }

        public void ForceSetState(bool state)
        {
            _curState = state;

            if(_curState == true)
            {
                daOn?.PlayImmediate(null, true);
            }
            else
            {
                daOff.PlayImmediate(null, true);
            }
        }

        public void SetState(bool state)
        {
            _curState = state;
            UpdateState();
        }

        protected void UpdateState()
        {
            if(_curState == true)
            {
                daOn?.Play();
            }
            else
            {
                daOff.Play();
            }
            OnChangeSwitch?.Invoke(_curState);
        }

        private void OnButtonClicked()
        {
            SetState(!_curState);
        }
    }
}