using AtoGame.OtherModules.HUD;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TrickyBrain
{
    public class ConfirmPopup : BasePopup
    {
        [SerializeField] private Button _btnConfirm;
        [SerializeField] private Button _btnCancel;
        [SerializeField] private TextMeshProUGUI _txtConfirm;
        [SerializeField] private TextMeshProUGUI _txtCancel;
        [SerializeField] private string _defaultConfirmText;
        [SerializeField] private string _defaultCancelText;


        protected Action _onConfirm;
        protected Action _onCancel;

        protected override void Start()
        {
            base.Start();
            _btnConfirm.onClick.AddListener(OnConfirmButtonClicked);
            _btnCancel.onClick.AddListener(OnCancelButtonClicked);
        }

        protected override void ActiveFrame()
        {
            base.ActiveFrame();
            if(_txtCancel != null)
            {
                _txtCancel.text = _defaultCancelText;
            }
            if(_txtConfirm != null)
            {
                _txtConfirm.text = _defaultConfirmText;
            }
            _onConfirm = _onCancel = null;
        }

        private void OnConfirmButtonClicked()
        {
            Hide();
            _onConfirm?.Invoke();
        }

        private void OnCancelButtonClicked()
        {
            Hide();
            _onCancel?.Invoke();
        }

        public ConfirmPopup SetConfirmText(string text)
        {
            if(_txtConfirm != null)
            {
                _txtConfirm.text = text;
            }
            return this;
        }

        public ConfirmPopup SetCancelText(string text)
        {
            if(_txtCancel != null)
            {
                _txtCancel.text = text;
            }
            return this;
        }

        public ConfirmPopup SetOnConfirm(Action onConfirm)
        {
            this._onConfirm = onConfirm;
            return this;
        }

        public ConfirmPopup SetOnCancel(Action onCancel)
        {
            this._onCancel = onCancel;
            return this;
        }
    }
}
