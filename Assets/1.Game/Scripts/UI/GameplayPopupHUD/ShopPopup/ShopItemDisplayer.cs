using AtoGame.Base.UI;
using AtoGame.OtherModules.LocalSaveLoad;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TrickyBrain
{
    public class ShopItemDisplayer : SelectableDisplayer<ShopItemConfigData>
    {
        [SerializeField] private Image imgBG;
        [SerializeField] private Image imgIcon;
        [SerializeField] private GameObject goUsing;
        [SerializeField] private GameObject goLevel;
        [SerializeField] private TextMeshProUGUI txtUnlockLevel;
        [SerializeField] private GameObject goAds;

        [Header("Configs")]
        [SerializeField] private Sprite spUnlockBG;
        [SerializeField] private Sprite spLockBG;
        [SerializeField] private Color clUnlockIcon;
        [SerializeField] private Color clLockIcon;

        public override void Show()
        {
            if(Model == null)
            {
                return;
            }
            var config = DataConfigs.Instance.ShopConfigData;
            var saveData = LocalSaveLoadManager.Get<ShopSaveData>();

            bool isUnlocked = Model.IsUnlocked;
            bool isUsing = Model.Id == saveData.UsingItemID;

            imgBG.sprite = isUnlocked ? spUnlockBG : spLockBG;
            imgIcon.sprite = Model.Icon;
            goUsing.SetActive(isUsing);

            if(isUnlocked == false)
            {
                bool useLevel = Model.UnlockLevel > 0;
                goLevel.SetActive(useLevel);
                if(useLevel)
                {
                    txtUnlockLevel.text = $"LV {Model.UnlockLevel}";
                }
                bool useAds = Model.UnlockLevel == -1;
                goAds.SetActive(useAds);
                imgIcon.color = clLockIcon;
            }
            else
            {
                goLevel.SetActive(false);
                goAds.SetActive(false);
                imgIcon.color = clUnlockIcon;
            }
           
        }
    }
}
