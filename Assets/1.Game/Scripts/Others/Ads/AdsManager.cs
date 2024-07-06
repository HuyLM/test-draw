using AtoGame.Base;
using AtoGame.Mediation;
using AtoGame.OtherModules.LocalSaveLoad;
using Falcon;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using TrickyBrain;
using UnityEngine;

namespace TrickyBrain
{
    public class AdsManager : AtoGame.Base.SingletonBindAlive<AdsManager>
    {
        [SerializeField] private string androidAppOpenAdId = "ca-app-pub-9819920607806935/3673960666";
        [SerializeField] private string iosAppOpenAdId = "ca-app-pub-9819920607806935/5039847370";

        [SerializeField] private string androidBannerAdId = "ca-app-pub-9819920607806935/3673960666";
        [SerializeField] private string iosAppBannerAdId = "ca-app-pub-9819920607806935/5039847370";
        [Header("Interstitial")]
        [SerializeField] private float showInterstitialCapping = 3; // sec
        [Header("Banner")]
        [SerializeField] private float reshowBannerCapping = 20; //sec

        [Header("Native")]
        [SerializeField] private string androidNativeAdId = "ca-app-pub-3940256099942544/2247696110";
        [SerializeField] private string iosNativeAdId = "ca-app-pub-3940256099942544/2247696110";


        private bool isShowing;
        private Action onRewardVideoLoadCompleted;

        AdmobBannerAd _admobBannerAd;
        private float reshowBannerCountdown;
        private bool isCountdownBanner;


        private AdmobAppOpenAd admobAppOpenAd;
        private DateTime showAOALastTime;
        private Action onAppOpenShowCompleted;
        private bool canShowAOA;

        private float lasttimeShowInterstitial;

        //private AdmobNativeAd admobNativeAd;
        private Action OnNativeLoadSuccess;

        public Action OnClaimedRewardVideo { get; set; }
        public string AppOpenAdId
        {
            get
            {
#if UNITY_ANDROID
#if TEST_AD
                return "ca-app-pub-3940256099942544/9257395921";
#else
                return androidAppOpenAdId;
#endif
#elif UNITY_IPHONE

#if TEST_AD
                return "ca-app-pub-3940256099942544/5575463023";
#else
              return iosAppOpenAdId;
#endif

#else
            return string.Empty;
#endif
            }
        }

        public string BannerAdId
        {
            get
            {
#if UNITY_ANDROID
#if TEST_AD
                return "ca-app-pub-3940256099942544/2014213617";
#else
                return androidBannerAdId;
#endif
#elif UNITY_IPHONE

#if TEST_AD
                return "ca-app-pub-3940256099942544/8388050270";
#else
              return iosAppBannerAdId;
#endif

#else
            return string.Empty;
#endif
            }
        }

        public string NativeAdId
        {
            get
            {
#if UNITY_ANDROID
#if TEST_AD
                return "ca-app-pub-3940256099942544/2247696110";
#else
                return androidNativeAdId;
#endif
#elif UNITY_IPHONE

#if TEST_AD
                return "ca-app-pub-3940256099942544/2247696110";
#else
              return iosNativeAdId;
#endif

#else
            return string.Empty;
#endif
            }
        }

        public bool IsAppOpenAdAvailable
        {
            get
            {
#if NO_ADS
                return false;
#endif
#if UNITY_EDITOR
                return false;
#endif
                if(isShowing)
                {
                    return false;
                }

                if(canShowAOA == false)
                {
                    return false;
                }

                /*
                if(GameManager.Instance.CanShowAOAByRemote == false)
                {
                    Debug.LogError("GameManager.Instance.CanShowAOAByRemote == false");
                }
                */

                if(admobAppOpenAd == null)
                {
                    Debug.LogError("admobAppOpenAd == null");
                    return false;
                }
                if(admobAppOpenAd.IsAvailable == false)
                {
                    return false;
                }
                return true;
            }
        }

        public override void Preload()
        {
            base.OnAwake();
            AdMediation.Init();
            //admobNativeAd = new AdmobNativeAd(NativeAdId);
        }

        public void InitAdmob()
        {
            // Initialize the Google Mobile Ads SDK.
            GoogleMobileAds.Api.MobileAds.Initialize((GoogleMobileAds.Api.InitializationStatus initStatus) =>
            {
                // This callback is called once the MobileAds SDK is initialized.
                Debug.Log("[AdsManager]-GoogleMobileAds.Api.MobileAds.Initialize with status: " + initStatus);
                if(initStatus != null)
                {

                    canShowAOA = true;
                    var adsSaveData = LocalSaveLoadManager.Get<AdsSaveData>();
                    if(adsSaveData != null)
                    {
                        canShowAOA = adsSaveData.IsFirstShowAOA == false;
                    }
                    if(canShowAOA == false)
                    {
                        LocalSaveLoadManager.Get<AdsSaveData>().IsFirstShowAOA = false;
                        LocalSaveLoadManager.Get<AdsSaveData>().SaveData();
                    }
                    admobAppOpenAd = new AdmobAppOpenAd(AppOpenAdId);
                    admobAppOpenAd.Request();

                    CreateBannerView();

                    /*
                    if(admobNativeAd == null)
                    {
                        admobNativeAd = new AdmobNativeAd(NativeAdId);
                    }
                    admobNativeAd.OnLoadSuccess = OnNativeAdLoadSuccess;
                    admobNativeAd.Request();
                    */
                }
                showAOALastTime = DateTime.Now;
            });
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            AdMediation.onInterstitialLoadedEvent += OnInterstitialLoadCompleted;
            AdMediation.onInterstitialLoadFailed += OnInterstitialLoadFailed;
            AdMediation.onInterstitiaClicked += OnInterstitialClicked;

            AdMediation.onVideoRewardLoadedEvent += OnRewardVideoLoadCompleted;
            AdMediation.onAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            AdMediation.onVideoRewardClicked += OnRewardVideoClicked;
            AdMediation.onVideoRewardDisplayedEvent += OnRewardVideoShowSuccess;
            AdMediation.onVideoRewardFailedEvent += OnRewardVideoFailed;
            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        }


        #region Interstitial

        public bool IsInterstitialReady()
        {
#if NO_ADS
            return false;
#endif
            AdsSaveData adsSaveData = LocalSaveLoadManager.Get<AdsSaveData>();
            if(adsSaveData.IsRemoveAds)
            {
                return false;
            }
            if(Time.realtimeSinceStartup - lasttimeShowInterstitial < showInterstitialCapping)
            {
                return false;
            }
#if UNITY_EDITOR
            return true;
#endif
            bool ready = AdMediation.IsInterstitialAvailable();
            return ready;
        }

        public void ShowInterstitial(string placement, Action onCompleted = null, Action onFailed = null)
        {
#if UNITY_EDITOR
            isShowing = false;
            onCompleted?.Invoke();
            return;
#endif
            lasttimeShowInterstitial = Time.realtimeSinceStartup;
            isShowing = true;
            GameTracking.LogCallShowInter();
            AdMediation.ShowInterstitial((adId, adInfo) =>
            {
                isShowing = false;
                onCompleted?.Invoke();
                GameTracking.LogAdInterShow();
                var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
                GameTracking.LogAdsLog(saveData.GetCurrentUnlockLevelIndex(), Falcon.FalconAnalytics.Scripts.Enum.AdType.Interstitial, placement);
            }, (title, desc) =>
            {
                isShowing = false;
                onFailed?.Invoke();
            });
        }

        private void OnInterstitialLoadCompleted(AdInfo adInfo)
        {
            GameTracking.LogAdInterLoadSuccess();
        }

        private void OnInterstitialLoadFailed(string error)
        {
            GameTracking.LogAdsInterLoadFail();
        }

        private void OnInterstitialClicked(AdInfo adInfo)
        {
            GameTracking.LogAdInterClick();
        }


        #endregion

        #region Video Reward

        public bool IsRewardVideoReady()
        {
#if NO_ADS
            return true;
#endif
#if UNITY_EDITOR
            return true;
#endif
            return AdMediation.IsRewardVideoAvailable();
        }

        public void ShowRewardVideo(string placement, Action onCompleted = null, Action onFailed = null)
        {
#if NO_ADS
            isShowing = false;
            onCompleted?.Invoke();
            OnClaimedRewardVideo?.Invoke();
            return;
#endif
#if UNITY_EDITOR
            isShowing = false;
            onCompleted?.Invoke();
            OnClaimedRewardVideo?.Invoke();
            return;
#endif
            isShowing = true;
            GameTracking.LogCallShowReward();
            AdMediation.ShowRewardVideo((adId, adInfo) =>
            {
                isShowing = false;
                onCompleted?.Invoke();
                OnClaimedRewardVideo?.Invoke();
                GameTracking.LogAdsRewardComplete();
                var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
                GameTracking.LogAdsLog(saveData.GetCurrentUnlockLevelIndex(), Falcon.FalconAnalytics.Scripts.Enum.AdType.Reward, placement);
            }, (title, desc) =>
            {
                isShowing = false;
                onFailed?.Invoke();
            });
        }

        public void AddOnRewardVideoReady(Action onReady)
        {
            onRewardVideoLoadCompleted += onReady;
        }

        public void RemoveOnRewardVideoReady(Action onReady)
        {
            onRewardVideoLoadCompleted -= onReady;
        }
        public void RemoveAllOnRewardVideoReady()
        {
            onRewardVideoLoadCompleted = null;
        }
        private void OnRewardVideoLoadCompleted(AdInfo adInfo)
        {
            onRewardVideoLoadCompleted?.Invoke();
            GameTracking.LogAdsRewardLoad();
        }

        private void OnRewardVideoClicked(string placement, AdInfo adInfo)
        {
            GameTracking.LogAdsRewardClick();
        }

        private void OnRewardVideoShowSuccess(AdInfo adInfo)
        {
            GameTracking.LogAdsRewardShowSuccess();
        }

        private void OnRewardVideoFailed(string placement, AdInfo adInfo)
        {
            GameTracking.LogAdsRewardShowFail();
        }

        #endregion

        #region Banner

        public void CreateBannerView()
        {
            _admobBannerAd = new AdmobBannerAd(BannerAdId, null, AdPosition.Bottom, true);
        }

        public void ShowBanner()
        {
#if NO_ADS
            return;
#endif
            AdsSaveData adsSaveData = LocalSaveLoadManager.Get<AdsSaveData>();
            if(adsSaveData.IsRemoveAds)
            {
                return;
            }

            GameTracking.LogCallShowBanner();

            // create an instance of a banner view first.
            if(_admobBannerAd == null)
            {
                CreateBannerView();
            }
            if(_admobBannerAd != null && _admobBannerAd.IsAvailable)
            {
                _admobBannerAd.Show(
                    (placment, adInfo) => {
                        ReshowBanner();
                        GameTracking.LogAdsLog(-1, Falcon.FalconAnalytics.Scripts.Enum.AdType.Banner, string.Empty);
                        GameTracking.LogDisplayBanner();
                    }
                    , (error, adInfo) => {
                        ReshowBanner();
                    });
            }
        }

        public void DestroyBanner()
        {
            // AdMediation.DestroyBanner();
            if(_admobBannerAd != null)
            {
                _admobBannerAd.DestroyBanner();
                _admobBannerAd = null;
            }

        }

        private void Update()
        {
            if(isCountdownBanner)
            {
                reshowBannerCountdown -= Time.deltaTime;
                if(reshowBannerCountdown < 0)
                {
                    isCountdownBanner = false;
                    ShowBanner();
                }
            }

        }

        public void ForceShowBanner()
        {
            if(isCountdownBanner == false)
            {
                ShowBanner();
            }
        }

        private void ReshowBanner()
        {
            if(isCountdownBanner == false)
            {
                reshowBannerCountdown = reshowBannerCapping;
                isCountdownBanner = true;
                Debug.Log("[AdsManager] ReshowBanner start countdown = " + reshowBannerCountdown);
            }
        }

        #endregion

        #region App Open Ad

        public void ShowAppOpenAd(Action onCompleted)
        {
            if(IsAppOpenAdAvailable)
            {
                onAppOpenShowCompleted = onCompleted;
                isShowing = true;
                showAOALastTime = DateTime.Now;

                Debug.Log("Showing app open ad.");
                admobAppOpenAd.Show((placment, adInfo) => {
                    onAppOpenShowCompleted?.Invoke();
                    GameTracking.LogAdsLog(-1, Falcon.FalconAnalytics.Scripts.Enum.AdType.AppOpen, string.Empty);
                }, (error, adInfo) => {
                    onAppOpenShowCompleted?.Invoke();
                });
            }
            else
            {
                Debug.LogError("[AdsManager] App open ad is not ready yet.");
                onCompleted?.Invoke();
            }
        }

        private void OnAppStateChanged(AppState state)
        {
            Debug.Log("App State changed to : " + state);

            // if the app is Foregrounded and the ad is available, show it.
            if(state == AppState.Foreground)
            {
                if((DateTime.Now - showAOALastTime).TotalSeconds < 20)
                {
                    return;
                }

                StartCoroutine(DelayShowAOA());
            }
        }

        private IEnumerator DelayShowAOA()
        {
            yield return null;
            ShowAppOpenAd(null);
        }

        #endregion

        /*
        #region Native

        public AdmobNativeAd NativeAd
        {
            get
            {
                if(admobNativeAd == null)
                {
                    admobNativeAd = new AdmobNativeAd(NativeAdId);
                }
                return admobNativeAd;
            }
        }

        private void OnNativeAdLoadSuccess()
        {
            OnNativeLoadSuccess?.Invoke();
        }

        public void AddOnNativeAdLoadSuccess(Action onReady)
        {
            OnNativeLoadSuccess += onReady;
        }

        public void RemoveOnNativeAdLoadSuccess(Action onReady)
        {
            OnNativeLoadSuccess -= onReady;
        }
        public void RemoveAllOnNativeAdLoadSuccess()
        {
            OnNativeLoadSuccess = null;
        }
        #endregion
        */

        private void OnAdRevenuePaidEvent(ImpressionData impressionData)
        {
            AtoGame.Tracking.ParameterBuilder parameterBuilder = AtoGame.Tracking.ParameterBuilder.Create()
            .Add("ad_platform", impressionData.adPlatform)
            .Add("ad_source", impressionData.adNetwork)
            .Add("ad_unit_name", impressionData.instanceName)
            .Add("ad_format", impressionData.adUnit)
            .Add("placement", impressionData.placement)
            .Add("value", impressionData.revenue.ToString())
            .Add("currency", "USD");
#if APPSFLYER_ADREVENUE_ENABLE
            AtoGame.Tracking.Appsflyer.AtoAppsflyerTracking.Instance.LogAdRevenue(impressionData.adNetwork,
                                                impressionData.adPlatform,
                                                impressionData.revenue.Value,
                                                "USD",
                                                parameterBuilder.BuildString());
#endif
            double revenue = impressionData.revenue.Value;
            var impressionParameters = new[] {
              new Firebase.Analytics.Parameter("ad_platform", impressionData.adPlatform),
              new Firebase.Analytics.Parameter("ad_source", impressionData.adNetwork),
              new Firebase.Analytics.Parameter("ad_unit_name", impressionData.instanceName),
              new Firebase.Analytics.Parameter("ad_format", impressionData.adUnit),
              new Firebase.Analytics.Parameter("value", revenue),
              new Firebase.Analytics.Parameter("currency", "USD"),
            };
            Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", parameterBuilder.BuildFirebase());

#if ADJUST_ENABLE
            AtoGame.Tracking.Adjust.AtoAdjustTracking.LogAdRevenue(
                impressionData.adNetwork,
                impressionData.adPlatform,
                revenue,
                impressionData.adUnit,
                impressionData.placement
                );
#endif
        }
    }
}
