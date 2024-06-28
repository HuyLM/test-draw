using AtoGame.Base.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TrickyBrain
{
    public class FindableObjectDisplayer : Displayer<FindableObject>
    {
        [SerializeField] private Image imgIcon;

        public override void Show()
        {
            if(Model == null)
            {
                imgIcon.gameObject.SetActive(false);
                imgIcon.sprite = null;
                imgIcon.color = Color.white;
                return;
            }
            imgIcon.gameObject.SetActive(true);
            imgIcon.sprite = Model.GetSprite();

        }
    }
}
