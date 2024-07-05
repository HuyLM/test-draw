using System.Collections;
using Falcon.FalconCore.Scripts.Controllers.Interfaces;
using UnityEngine;

public class FalconSDKInit : IFInit
{
    public IEnumerator Init()
    {
        GameObject obj = new GameObject("FalconFirebase");
        obj.AddComponent<FalconFirebase>();
        yield return null;
    }

    public static void AddAppsflyer()
    {
        GameObject obj = new GameObject("FalconAppsFlyer");
        obj.AddComponent<FalconAppsFlyerAndAdjust>();
    }
}