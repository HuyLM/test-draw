using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FalconAdjustSettings : ScriptableObject
{
    [Header("Adjust token")] [Tooltip("App token")]
    public string appToken = string.Empty;

    [Header("Event token")] [Tooltip("Interstitial show token")]
    public string eventInterstitialShowToken = string.Empty;

    [Tooltip("Interstitial displayed token")]
    public string eventInterstitialDisplayedToken = string.Empty;

    [Tooltip("Rewarded show token")] public string eventRewardedShowToken = string.Empty;

    [Tooltip("Interstitial displayed token")]
    public string eventRewardedDisplayedToken = string.Empty;

    public static Action validateEvent;

    private void OnValidate()
    {
        validateEvent?.Invoke();
    }

    public static FalconAdjustSettings LoadInstance()
    {
        //Read from resources.
        var instance = Resources.Load<FalconAdjustSettings>("FalconAdjustSettings");
        return instance;
    }
}