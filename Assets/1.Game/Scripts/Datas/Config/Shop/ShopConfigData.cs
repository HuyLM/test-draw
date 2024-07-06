using AtoGame.OtherModules.Inventory;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TrickyBrain
{
    [CreateAssetMenu(fileName = "ShopConfigData", menuName = "TrickyBrain/Configs/ShopConfigData")]
    public class ShopConfigData : BaseItemCollector
    {
        public List<ShopItemConfigData> ShopItemConfigDatas;

        public override List<ItemConfig> GetItems()
        {
            return ShopItemConfigDatas.Cast<ItemConfig>().ToList();
        }

        public ShopItemConfigData GetDefaultItem()
        {
            return ShopItemConfigDatas.FirstOrDefault(i => i.UnlockLevel == 0);
        }

        public void UnlockDefaultItems()
        {
            for(int i = 0; i < ShopItemConfigDatas.Count; i++)
            {
                if(ShopItemConfigDatas[i].UnlockLevel == 0)
                {
                    ShopItemConfigDatas[i].Claim(1, "unlock_default");
                }
            }
        }

        public ShopItemConfigData GetItem(int id)
        {
            for(int i = 0; i < ShopItemConfigDatas.Count; i++)
            {
                if(ShopItemConfigDatas[i].Id == id)
                {
                    return ShopItemConfigDatas[i];
                }
            }
            return null;
        }
    }

   
}
