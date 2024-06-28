using AtoGame.Base.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TrickyBrain
{
    public class LogoLoadingProgressBar : BaseProgressBar
    {
        [SerializeField] private TextMeshProUGUI txtProgressValue;

        public override void ForceFillBar(float pct)
        {
            base.ForceFillBar(pct);
            if (txtProgressValue != null)
            {
                txtProgressValue.text = $"{(int)(pct * 100)}%";
            }
        }
    }
}
