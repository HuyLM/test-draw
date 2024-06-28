using AtoGame.Base.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class FindableObjectCollector : DisplayerCollector<FindableObject, FindableObjectDisplayer>
    {
        public void Add(FindableObject[] findableObjects)
        {
            int startIndex = 0;
            for(int i = 0; i < Capacity; i++)
            {
                FindableObjectDisplayer displayer = GetDisplayer(i);
                if(displayer && displayer.Model == null)
                {
                    startIndex = i;
                    break;
                }
            }

            for(int i = 0; i < findableObjects.Length; ++i)
            {
                Items[startIndex + i] = findableObjects[i];
                FindableObjectDisplayer displayer = GetDisplayer(startIndex + i);
                SetupDisplayer(displayer, findableObjects[i]);
            }
        }
    }
}
