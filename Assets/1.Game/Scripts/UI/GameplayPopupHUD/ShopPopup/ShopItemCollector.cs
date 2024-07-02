using AtoGame.Base.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class ShopItemCollector : SelectableDisplayerCollector<ShopItemConfigData, ShopItemDisplayer>
    {
        public ShopItemDisplayer GetShopItemDipslayer(int id)
        {
            for(int i = 0; i < Capacity; i++)
            {
                var displayer = GetDisplayer(i);
                if(displayer != null && displayer.Model.Id == id)
                {
                    return displayer;
                }
            }
            return null;
        }
    }
}
