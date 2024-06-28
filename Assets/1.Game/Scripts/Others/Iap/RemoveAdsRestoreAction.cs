#if UNITY_IAP_ENABLE
using AtoGame.IAP;
#endif
using AtoGame.OtherModules.LocalSaveLoad;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    [CreateAssetMenu(fileName = "RemoveAdsRestoreAction", menuName = "ScriptableObjects/TrickyBrain/Iap/RemoveAdsRestoreAction")]
    public class RemoveAdsRestoreAction :
#if UNITY_IAP_ENABLE
        IapAction
#else
        ScriptableObject
#endif
    {
#if UNITY_IAP_ENABLE
        public override void Execute(string target, object user = null, Action onCompleted = null)
        {
            AdsSaveData adsSaveData = LocalSaveLoadManager.Get<AdsSaveData>();
            if(adsSaveData.IsRemoveAds == false)
            {
                adsSaveData.IsRemoveAds = true;
                adsSaveData.SaveData();
                onCompleted?.Invoke();
            }
        }
#endif
    }
}
