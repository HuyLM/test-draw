using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.Inventory
{
    public abstract class BaseItemCollector : ScriptableObject
    {
        [SerializeField] private string nameCollector = "other";
        [SerializeField] private List<ItemCollector> collectors;


        public virtual string NameCollector { get => nameCollector; }
        public virtual List<ItemCollector> Collectors => collectors;

        public abstract List<ItemConfig> GetItems();
       
    }
}
