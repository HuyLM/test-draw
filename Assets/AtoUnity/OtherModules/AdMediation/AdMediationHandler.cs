using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.Mediation
{
    public interface IAdMediationHandler
    {
        void Init();
        void ShowTestSuite();
        bool IsRewardVideoAvailable();
        void ShowRewardVideo(Action<string, AdInfo> onCompleted = null, Action<string, AdInfo> onFailed = null);
        void LoadRewardVideo();
        bool IsInterstitialAvailable();
        void ShowInterstitial(Action<string, AdInfo> onCompleted = null, Action<string, AdInfo> onFailed = null);
        void LoadInterstitial();
        void ShowBanner(Action<string, AdInfo> onCompleted = null, Action<string, AdInfo> onFailed = null);
        void ShowBanner(BannerPosition position, bool isAdaptive, BannerSize size, int width, int height, Action<string, AdInfo> onCompleted = null, Action<string, AdInfo> onFailed = null);
        void DestroyBanner();
        void HideBanner();
        void DisplayBanner();
    }

    public class DefaultAdMediationHandler : IAdMediationHandler
    {
        public void Init()
        {
        }
        public void ShowTestSuite()
        {
        }
        public bool IsRewardVideoAvailable()
        {
            return false;
        }
        public void ShowRewardVideo(Action<string, AdInfo> onCompleted = null, Action<string, AdInfo> onFailed = null)
        {
        }
        public void LoadRewardVideo()
        {
        }
        public bool IsInterstitialAvailable()
        {
            return false;
        }
        public void ShowInterstitial(Action<string, AdInfo> onCompleted = null, Action<string, AdInfo> onFailed = null)
        {
        }
        public void LoadInterstitial()
        {
        }
        public void ShowBanner(Action<string, AdInfo> onCompleted = null, Action<string, AdInfo> onFailed = null)
        {
        }

        public void ShowBanner(BannerPosition position, bool isAdaptive, BannerSize size, int width, int height, Action<string, AdInfo> onCompleted = null, Action<string, AdInfo> onFailed = null)
        {
        }

        public void DestroyBanner()
        {
        }

        public void DisplayBanner()
        {
        }

        public void HideBanner()
        {
        }
    }
}
