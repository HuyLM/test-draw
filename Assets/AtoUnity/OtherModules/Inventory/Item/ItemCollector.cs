using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.Inventory
{
    [CreateAssetMenu(fileName = "ItemCollector", menuName = "Data/OtherModules/Inventory/ItemCollector")]
    public class ItemCollector : BaseItemCollector
    {
        [SerializeField] private List<ItemConfig> items;

        public override List<ItemConfig> GetItems()
        {
            return items;
        }
     

#if UNITY_EDITOR
        public void LockAllItem()
        {
            foreach (var i in items)
            {
                i.lockID = true;
                UnityEditor.EditorUtility.SetDirty(i);
            }
            foreach (var c in Collectors)
            {
                c.LockAllItem();
            }
        }
#endif
    }
}
