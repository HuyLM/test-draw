using AtoGame.Base.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class UseableObjectCollector : DisplayerCollector<UseableObject, UseableObjectDisplayer>
    {
        [SerializeField] private UseableDrager dragger;
        private bool ignoreInput;

        public void Init()
        {
            dragger.gameObject.SetActive(false);
        }

        public void Add(UseableObject[] useableObjects)
        {
            int preCapacity = Capacity;
            Capacity += useableObjects.Length;
            if(Items == null)
            {
                Items = new List<UseableObject>();
            }
            Items.AddRange(useableObjects);

            for(int i = preCapacity; i < Capacity; i++)
            {
                if(DisplayerCount == i)
                {
                    AddDisplayer(CreateDisplayer());
                }

                UseableObjectDisplayer displayer = GetDisplayer(i);
                if(displayer)
                {
                    displayer.gameObject.SetActive(true);
                    SetupDisplayer(displayer, GetItem(i));
                    displayer.IgnoreInput(ignoreInput);
                }
            }
        }

        public void Remove(UseableObject[] useableObjects)
        {
            for(int i = 0; i < useableObjects.Length; ++i)
            {
                for(int j = 0; j < Capacity; ++j)
                {
                    var displayer = GetDisplayer(j);
                    if(displayer.Model == useableObjects[i])
                    {
                        Capacity--;
                        Items.Remove(displayer.Model);
                        break;
                    }
                }
            }
            Show();
        }

        public void Replace(UseableObject from, UseableObject to)
        {
            for(int i = 0; i < Capacity; i++)
            {
                UseableObjectDisplayer displayer = GetDisplayer(i);
                if(displayer != null && displayer.Model == from)
                {
                    displayer.SetModel(to).Show();
                }
            }
        }

        public void IgnoreInput(bool ignoreEnable)
        {
            this.ignoreInput = ignoreEnable;
            for(int i = 0; i < Capacity; i++)
            {
                UseableObjectDisplayer displayer = GetDisplayer(i);
                if(displayer)
                {
                    displayer.IgnoreInput(ignoreEnable);
                }
            }
        }


    }
}
