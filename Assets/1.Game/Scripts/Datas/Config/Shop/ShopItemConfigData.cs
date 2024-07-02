using AtoGame.OtherModules.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    [CreateAssetMenu(fileName = "ShopItemConfigData", menuName = "TrickyBrain/Configs/ShopItemConfigData")]
    public class ShopItemConfigData : ItemConfig
    {
        public int UnlockLevel; // -1 unlock by Ads , 0 default unlock

        public bool IsUnlocked
        {
            get
            {
                return GetAvaliable() > 0;
            }
        }
    }
}
