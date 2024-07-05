using System;
using Falcon.FalconCore.Scripts.FalconABTesting.Scripts.Model;
using Falcon.FalconGoogleUMP;
using UnityEngine;
#if UNITY_IOS
using System.Collections;
using Unity.Advertisement.IosSupport;
#endif

public class PopupConsent : MonoBehaviour
{
    public static Action OnMediationInitialized;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        FalconUMP.ShowConsentForm(InitMediation, null, onShowPopupATT);
    }

    private void onShowPopupATT()
    {
#if UNITY_IOS
        //show popup ATT
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
        StartCoroutine(WaitUntilDetermined());
#elif UNITY_ANDROID
#if USE_APPSFLYER || USE_ADJUST
        FalconSDKInit.AddAppsflyer();
#endif
#endif
    }
#if UNITY_IOS
    IEnumerator WaitUntilDetermined()
    {
        yield return new WaitUntil(() =>
            ATTrackingStatusBinding.GetAuthorizationTrackingStatus() !=
            ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED);
        FalconSDKInit.AddAppsflyer();
    }
#endif
    private void InitMediation(bool consent)
    {
        /*
        var settings = Resources.Load<Falcon.FalconMediation.Core.FalconMediationSettings>("FalconMediationSettings");
        if (settings == null)
        {
            Debug.LogError(
                "You have to set up first. In Unity menu bar, Click Falcon > FalconMediation to open the settings file.");
            return;
        }

        Falcon.FalconMediation.Core.FalconMediationCore.InitCore(consent, settings, OnMediationInitialized);
        */
    }
}