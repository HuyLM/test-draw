﻿
using UnityEngine;

namespace AtoGame.Base
{
    public class SingletonScriptableObjectReference : MonoBehaviour
    {
        [SerializeField] private SingletonScriptableObject[] scriptableObjects;

        public void Awake()
        {
            for (int i = 0; i < scriptableObjects.Length; ++i)
            {
                scriptableObjects[i].OnAwake();
            }
        }
    }
}
