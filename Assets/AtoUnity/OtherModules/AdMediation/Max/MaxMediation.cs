using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.Mediation
{
    #if ATO_MAX_MEDIATION_ENABLE
    public class MaxMediation : SingletonFreeAlive<MaxMediation>, IAdMediationHandler
    {
        [SerializeField] private bool setMute;
        [SerializeField] private bool showCreativeDebugger;
        [SerializeField] private string sdkKey;
        [Header("Android Keys")]
        [SerializeField] private string androidInterstitialAdUnitId = "ENTER_ANDROID_INTERSTITIAL_AD_UNIT_ID_HERE";
        [SerializeField] private string androidRewardedAdUnitId = "ENTER_ANDROID_REWARD_AD_UNIT_ID_HERE";
        [SerializeField] private string androidBannerAdUnitId = "ENTER_ANDROID_BANNER_AD_UNIT_ID_HERE";
        [SerializeField] private string androidMRecAdUnitId = "ENTER_ANDROID_MREC_AD_UNIT_ID_HERE";
        [Header("Ios Keys")]
        [SerializeField] private string iosInterstitialAdUnitId = "ENTER_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
        [SerializeField] private string iosRewardedAdUnitId = "ENTER_IOS_REWARD_AD_UNIT_ID_HERE";
        [SerializeField] private string iosBannerAdUnitId = "ENTER_IOS_BANNER_AD_UNIT_ID_HERE";
        [SerializeField] private string iosMRecAdUnitId = "ENTER_IOS_MREC_AD_UNIT_ID_HERE";
        [Space(20)]
        [Header("Default Banner")]
        [SerializeField] private bool useAdaptiveBanner = true;
        [SerializeField] private BannerPosition defaultBannerPosition = BannerPosition.BOTTOM_CENTER;
        [SerializeField] private BannerSize bannerSize;
        [SerializeField] private int bannerWidth;
        [SerializeField] private int bannerHeight;
        [SerializeField, ColorUsage(true)] private Color backgroundColor;
        [Header("Privacy")]
        [SerializeField] private bool hasUserConsent = true;
        [SerializeField] private bool isAgeRestrictedUser = false;
        [SerializeField] private bool doNotSell = false;
        [SerializeField] private bool isLimitedDataUse = false;
        [SerializeField] private bool useFacebookAd = false;

        private MaxVideoRewardAd rewardAd;
        private MaxInterstitialAd interstitialAd;
        private BaseMaxBannerAd bannerAd;
        private bool isInitialized;

        private string InterstitialAdUnitId
        {
            get
            {
#if UNITY_IOS
                return iosInterstitialAdUnitId;
#endif
                return androidInterstitialAdUnitId;
            }
        }
        private string RewardedAdUnitId
        {
            get
            {
#if UNITY_IOS
                return iosRewardedAdUnitId;
#endif
                return androidRewardedAdUnitId;
            }
        }
        private string BannerAdUnitId
        {
            get
            {
#if UNITY_IOS
                return iosBannerAdUnitId;
#endif
                return androidBannerAdUnitId;
            }
        }
        private string MRecAdUnitId
        {
            get
            {
#if UNITY_IOS
                return iosMRecAdUnitId;
#endif
                return androidMRecAdUnitId;
            }
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            AdMediation.SetHandler(this);
        }

        public void Init()
        {
            isInitialized = false;
            rewardAd = new MaxVideoRewardAd(RewardedAdUnitId);
            interstitialAd = new MaxInterstitialAd(InterstitialAdUnitId);

            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitializedEvent;

            // Privacy
            MaxSdk.SetHasUserConsent(hasUserConsent);
            MaxSdk.SetIsAgeRestrictedUser(isAgeRestrictedUser);
            MaxSdk.SetDoNotSell(doNotSell);
            if(useFacebookAd)
            {
                if (isLimitedDataUse)
                {
                    AudienceNetwork.AdSettings.SetDataProcessingOptions(new string[] { "LDU" }, 1, 1000);
                }
                else
                {
                    AudienceNetwork.AdSettings.SetDataProcessingOptions(new string[] { });
                }
            }

            MaxSdk.SetSdkKey(sdkKey);
            MaxSdk.InitializeSdk();
        }

        private void OnSdkInitializedEvent(MaxSdkBase.SdkConfiguration config)
        {
#if UNITY_IOS || UNITY_IPHONE || UNITY_EDITOR
            if (MaxSdkUtils.CompareVersions(UnityEngine.iOS.Device.systemVersion, "14.5") != MaxSdkUtils.VersionComparisonResult.Lesser)
            {
                // Note that App transparency tracking authorization can be checked via `sdkConfiguration.AppTrackingStatus` for Unity Editor and iOS targets
                // 1. Set Meta ATE flag here, THEN
                if(config.AppTrackingStatus == MaxSdkBase.AppTrackingStatus.Authorized)
                {
                    if(useFacebookAd)
                    {
                        AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(true);
                    }
                }
                Debug.Log("ATE is " + MaxSdkBase.AppTrackingStatus.Authorized.ToString());
            }
#endif
         
            Debug.Log("[AdMediation-MaxMediation] I got OnSdkInitializedEvent");
            isInitialized = true;
            MaxSdk.SetMuted(setMute);
            MaxSdk.SetCreativeDebuggerEnabled(showCreativeDebugger);
            LoadInterstitial();
            LoadRewardVideo();
        }

        public void ShowTestSuite()
        {
            if(isInitialized)
            {
                MaxSdk.ShowMediationDebugger();
            }
        }

#region Video Reward
        public bool IsRewardVideoAvailable()
        {
            return rewardAd != null && rewardAd.IsAvailable;
        }

        public void ShowRewardVideo(Action<string, AdInfo> onCompleted = null, Action<string, AdInfo> onFailed = null)
        {
            if (IsRewardVideoAvailable())
            {
                rewardAd.Show(onCompleted, onFailed);
            }
            else
            {
                onFailed?.Invoke("Reward Ad not available", null);
            }
        }

        public void LoadRewardVideo()
        {
            rewardAd?.Request();
        }
        #endregion

        #region Interstitial
        public bool IsInterstitialAvailable()
        {
            return interstitialAd != null && interstitialAd.IsAvailable;
        }

        public void ShowInterstitial(Action<string, AdInfo> onCompleted = null, Action<string, AdInfo> onFailed = null)
        {
            if (IsInterstitialAvailable())
            {
                interstitialAd.Show(onCompleted, onFailed);
            }
            else
            {
                onFailed?.Invoke("Interstitial Ad not available", null);
            }
        }

        public void LoadInterstitial()
        {
            interstitialAd?.Request();
        }
        #endregion

        #region Banner

        public void ShowBanner(Action<string, AdInfo> onCompleted = null, Action<string, AdInfo> onFailed = null)
        {
            if(bannerAd != null)
            {
                bannerAd.Destroy();
                bannerAd = null;
            }

            if(bannerSize == BannerSize.RECTANGLE)
            {
                bannerAd = new MaxMRECAd(MRecAdUnitId, MaxHelper.GetViewPosition(defaultBannerPosition));
            }
            else if(bannerSize == BannerSize.BANNER)
            {
                bannerAd = new MaxBannerAd(BannerAdUnitId, MaxHelper.GetBannerPosition(defaultBannerPosition), useAdaptiveBanner, backgroundColor);
            }
            
            if(bannerAd != null)
            {
                bannerAd.Show(onCompleted, onFailed);
            }
            else
            {
                onFailed?.Invoke("bannerAd null", null);
            }
        }

        public void ShowBanner(BannerPosition position, bool isAdaptive, BannerSize size, int width, int height, Action<string, AdInfo> onCompleted = null, Action<string, AdInfo> onFailed = null)
        {
            if (bannerAd != null)
            {
                bannerAd.Destroy();
                bannerAd = null;
            }

            if (size == BannerSize.RECTANGLE)
            {
                bannerAd = new MaxMRECAd(MRecAdUnitId, MaxHelper.GetViewPosition(position));
            }
            else if (bannerSize == BannerSize.BANNER)
            {
                bannerAd = new MaxBannerAd(BannerAdUnitId, MaxHelper.GetBannerPosition(position), isAdaptive, backgroundColor);
            }

            if (bannerAd != null)
            {
                bannerAd.Show(onCompleted, onFailed);
            }
            else
            {
                onFailed?.Invoke("bannerAd null", null);
            }
        }

        public void DestroyBanner()
        {
            if (bannerAd != null)
            {
                bannerAd.Destroy();
            }
        }

        public void HideBanner()
        {
            if (bannerAd != null)
            {
                bannerAd.Hide();
            }
        }

        public void DisplayBanner()
        {
            if (bannerAd != null)
            {
                bannerAd.Display();
            }
        }
        #endregion

        public void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            AdMediation.onAdRevenuePaidEvent?.Invoke(adInfo.ConvertToImpression());
        }
    }
#endif
}
