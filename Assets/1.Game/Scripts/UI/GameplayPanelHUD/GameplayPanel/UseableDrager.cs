using AtoGame.Base.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TrickyBrain
{
    public class UseableDrager : Displayer<UseableObject>
    {
        [SerializeField] private Image imgIcon;
        public override void Show()
        {
            if(Model == null)
            {
                return;
            }
            imgIcon.sprite = Model.Sprite;
        }
    }
}
