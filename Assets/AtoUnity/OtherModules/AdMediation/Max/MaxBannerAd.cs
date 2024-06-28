using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.Mediation
{
#if ATO_MAX_MEDIATION_ENABLE
    public class MaxBannerAd : BaseMaxBannerAd
    {
        private string adUnitId;
        private MaxSdkBase.BannerPosition position;
        private bool isAdaptive;
        private Color backgroundColor;


        public override bool IsAvailable
        {
            get
            {
                return true;
            }
        }

        public MaxBannerAd(string adUnitId, MaxSdkBase.BannerPosition position, bool isAdaptive, Color backgroundColor)
        {
            this.adUnitId = adUnitId;
            this.position = position;
            this.isAdaptive = isAdaptive;
            this.backgroundColor = backgroundColor;
            CallAddEvent();
        }

        protected override void CallAddEvent()
        {
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
            MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
            MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;

        }

        protected override void CallRequest()
        {
            Debug.Log("MaxBannerAd can't request");
        }

        protected override void CallShow()
        {
            MaxSdk.CreateBanner(adUnitId, position);
            MaxSdk.SetBannerExtraParameter(adUnitId, "adaptive_banner", isAdaptive ? "true" : "false");
            MaxSdk.SetBannerBackgroundColor(adUnitId, backgroundColor);
            MaxSdk.ShowBanner(adUnitId);
        }

        public void DestroyBanner()
        {
            MaxSdk.DestroyBanner(adUnitId);
            MaxSdkCallbacks.Banner.OnAdLoadedEvent -= OnBannerAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent -= OnBannerAdLoadFailedEvent;
            MaxSdkCallbacks.Banner.OnAdClickedEvent -= OnBannerAdClickedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent -= OnBannerAdRevenuePaidEvent;
            MaxSdkCallbacks.Banner.OnAdExpandedEvent -= OnBannerAdExpandedEvent;
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent -= OnBannerAdCollapsedEvent;
        }

        public void HideBanner()
        {
            MaxSdk.HideBanner(adUnitId);
        }

        public void DisplayerBanner()
        {
            MaxSdk.ShowBanner(adUnitId);
        }

        public override void Destroy()
        {
            DestroyBanner();
        }

        public override void Hide()
        {
            HideBanner();
        }

        public override void Display()
        {
            DisplayerBanner();
        }



#region Listners

        private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            OnCompleted(true, adInfo.Placement, adInfo.Convert());
            AdMediation.onBannerCompletedEvent?.Invoke(adInfo.Placement, adInfo.Convert());
            Debug.Log($"[AdMediation-MaxBannerAd]: {adUnitId} got OnBannerAdLoadedEvent With AdInfo " + adInfo.ToString());

            Debug.Log("Waterfall Name: " + adInfo.WaterfallInfo.Name + " and Test Name: " + adInfo.WaterfallInfo.TestName);
            Debug.Log("Waterfall latency was: " + adInfo.WaterfallInfo.LatencyMillis + " milliseconds");
            string waterfallInfoStr = "";
            foreach (var networkResponse in adInfo.WaterfallInfo.NetworkResponses)
            {
                waterfallInfoStr = "Network -> " + networkResponse.MediatedNetwork +
                                   "\n...adLoadState: " + networkResponse.AdLoadState +
                                   "\n...latency: " + networkResponse.LatencyMillis + " milliseconds" +
                                   "\n...credentials: " + networkResponse.Credentials;
            }
            Debug.Log(waterfallInfoStr);
        }

        private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) 
        {
            OnAdLoadFailed(errorInfo.ToString());
            AdMediation.onBannerFailedEvent?.Invoke(errorInfo.ToString());
            Debug.Log($"[AdMediation-MaxBannerAd]: {adUnitId} got OnBannerAdLoadFailedEvent With AdInfo " + errorInfo.ToString());

            Debug.Log("Waterfall Name: " + errorInfo.WaterfallInfo.Name + " and Test Name: " + errorInfo.WaterfallInfo.TestName);
            Debug.Log("Waterfall latency was: " + errorInfo.WaterfallInfo.LatencyMillis + " milliseconds");

            foreach (var networkResponse in errorInfo.WaterfallInfo.NetworkResponses)
            {
                Debug.Log("Network -> " + networkResponse.MediatedNetwork +
                      "\n...latency: " + networkResponse.LatencyMillis + " milliseconds" +
                      "\n...credentials: " + networkResponse.Credentials +
                      "\n...error: " + networkResponse.Error);
            }
        }

        private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log($"[AdMediation-MaxBannerAd]: {adUnitId} got OnBannerAdClickedEvent With AdInfo " + adInfo.ToString());
        }

        private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            OnAdOpening(adInfo.ConvertToImpression());
            Debug.Log($"[AdMediation-MaxBannerAd]: {adUnitId} got OnBannerAdRevenuePaidEvent With AdInfo " + adInfo.ToString());
        }

        private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) 
        {
            Debug.Log($"[AdMediation-MaxBannerAd]: {adUnitId} got OnBannerAdExpandedEvent With AdInfo " + adInfo.ToString());
        }

        private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) 
        {
            Debug.Log($"[AdMediation-MaxBannerAd]: {adUnitId} got OnBannerAdCollapsedEvent With AdInfo " + adInfo.ToString());
        }

#endregion
    }
#endif
}
