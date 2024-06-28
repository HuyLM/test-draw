using AtoGame.OtherModules.HUD;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TrickyBrain
{
    public class NoticePopup : BasePopup
    {
        [SerializeField] private Button _btnConfirm;
        [SerializeField] private TextMeshProUGUI _txtConfirm;
        [SerializeField] private string _defaultConfirmText;

        protected Action _onConfirm;

        protected override void Start()
        {
            base.Start();
            _btnConfirm.onClick.AddListener(OnConfirmButtonClicked);
        }

        protected override void ActiveFrame()
        {
            base.ActiveFrame();
            if(_txtConfirm && string.IsNullOrEmpty(_defaultConfirmText) == false) _txtConfirm.text = _defaultConfirmText;
            _onConfirm = null;
        }

        protected virtual void OnConfirmButtonClicked()
        {
            Hide();
            _onConfirm?.Invoke();
        }

        public NoticePopup SetConfirmText(string text)
        {
            if(_txtConfirm)
                _txtConfirm.text = text;
            return this;
        }

        public NoticePopup SetOnConfirm(Action onConfirm)
        {
            this._onConfirm = onConfirm;
            return this;
        }
    }
}
