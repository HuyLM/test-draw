using AtoGame.Base.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TrickyBrain
{
    public class LevelDisplayer : SelectableDisplayer<LevelData>
    {
        [SerializeField] private TextMeshProUGUI txtNumber;
        [SerializeField] private Image imgLocked;
        [SerializeField] private Image imgBlack;
        [SerializeField] private Image imgCompleted;
        [SerializeField] private GameObject goHighlight;
        public override void Show()
        {
            if(Model == null)
            {
                return;
            }
            imgCompleted.gameObject.SetActive(false);
            imgBlack.gameObject.SetActive(false);
            imgLocked.gameObject.SetActive(false);
            txtNumber.gameObject.SetActive(false);

            if(Model.State == LevelState.Locked)
            {
                imgBlack.gameObject.SetActive(true);
                imgLocked.gameObject.SetActive(true);
            }
            else if(Model.State == LevelState.Unlocked)
            {
                txtNumber.gameObject.SetActive(true);
                txtNumber.text = (Model.Index + 1).ToString();
            }
            else if(Model.State == LevelState.Completed)
            {
                imgBlack.gameObject.SetActive(true);
                imgCompleted.gameObject.SetActive(true);
                txtNumber.gameObject.SetActive(true);
                txtNumber.text = (Model.Index + 1).ToString();
            }
        }

        public void SetHighlight(bool show)
        {
            if(goHighlight)
            {
                goHighlight.SetActive(show);
            }
        }
    }

}
