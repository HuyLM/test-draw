using AtoGame.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class EnableExcuteActionMono : MonoBehaviour
    {
        [SerializeField]
        private ActionMono[] actionMono;

        private void OnEnable()
        {
            if(actionMono != null )
            {
                int length = actionMono.Length;
                for(int i = 0; i < length; i++)
                {
                    if(actionMono != null)
                    {
                        actionMono[i].Execute();
                    }
                }
            }
        }
    }
}
