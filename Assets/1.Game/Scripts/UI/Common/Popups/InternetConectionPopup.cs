using AtoGame.OtherModules.HUD;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TrickyBrain
{
    public class InternetConectionPopup : BasePopup
    {
        [SerializeField] private Button _btnConfirm;
        [SerializeField] private Button _btnCancel;

        protected override void Start()
        {
            base.Start();
            _btnConfirm.onClick.AddListener(OnConfirm);
            _btnCancel.onClick.AddListener(OnConfirm);
        }

        private void OnConfirm()
        {
#if UNITY_EDITOR
            if(!OthersService.Instance.hasConnection)
            {
                ActiveFrame();
            }
            else
            {
                Close();
            }
#else
            if(!OthersService.HasInternet)
            {
                ActiveFrame();
            }
            else
            {
                Close();
            }
#endif
        }
    }
}
