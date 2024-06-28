using AtoGame.Base;
using AtoGame.Tracking;
using AtoGame.Tracking.Adjust;
using AtoGame.Tracking.Appsflyer;
using AtoGame.Tracking.FB;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AtoGame.OtherModules.Inventory.EventKey;

namespace TrickyBrain
{
    public static class GameTracking
    {
        public static void PreLoad()
        {
            return;
            AtoFirebaseTracking.Instance.Preload();
            AtoAdjustTracking.Instance.Preload();
            return;
            AtoAppsflyerTracking.Instance.Preload();
            EventDispatcher.Instance.AddListener<OnAddItemInventory>(OnAddItemInventory);
        }

        private static void OnAddItemInventory(OnAddItemInventory param)
        {
            int id = param.id;
            /*
            if (id == ItemInventoryConstantID.MEDAL_ID)
            {
                LogEarnVirtualCurrency(param.id, param.name, param.value, param.source);
            }
            */
        }

        private static void LogEvent(string eventName, ParameterBuilder parameterBuilder)
        {
            LogFirebase(eventName, parameterBuilder);
            LogAppsflyer(eventName, parameterBuilder);
            //LogAdjust(eventName, parameterBuilder);
        }

        private static void LogFirebase(string eventName, ParameterBuilder parameterBuilder)
        {
            return;
            AtoFirebaseTracking.Instance.LogEvent(eventName, parameterBuilder);
        }

        private static void LogAppsflyer(string eventName, ParameterBuilder parameterBuilder)
        {
            return;
            AtoAppsflyerTracking.Instance.LogEvent(eventName, parameterBuilder);
        }

        private static void LogAdjust(string eventName, ParameterBuilder parameterBuilder)
        {
            return;
        }

        public static void LogAdsRemove()
        {
            LogEvent("check_ads_remove", null);
        }

        public static void LogLevelStart(int levelIndex)
        {
            ParameterBuilder parameterBuilder = ParameterBuilder.Create()
             .Add("level", (levelIndex + 1).ToString("D2"));
            LogEvent("level_start", parameterBuilder);
            ParameterBuilder adjustParameterBuilder = ParameterBuilder.Create()
            .Add("aj_level", (levelIndex + 1));
            LogAdjust("7lmv95", adjustParameterBuilder);
        }

        public static void LogLevelComplete(int levelIndex)
        {
            ParameterBuilder parameterBuilder = ParameterBuilder.Create()
            .Add("level", (levelIndex + 1).ToString("D2"));
            LogEvent("level_complete", parameterBuilder);
        }

        

        public static void LogEarnVirtualCurrency(int id, string itemName, long value, string source)
        {
            ParameterBuilder parameterBuilder = ParameterBuilder.Create()
             .Add("virtual_currency_name", itemName)
             .Add("value", value)
             .Add("source", source);
            LogEvent("earn_virtual_currency", parameterBuilder);
        }

        public static void LogSpendVirtualCurrency(string itemName, long value, string boughtItemName, string source)
        {
            ParameterBuilder parameterBuilder = ParameterBuilder.Create()
             .Add("virtual_currency_name", itemName)
             .Add("value", value)
             .Add("item_name", boughtItemName)
             .Add("source", source);
            LogEvent("spend_virtual_currency", parameterBuilder);
        }

        public static void LogAdsRewardLoad()
        {
            LogEvent("ads_reward_load", null);
        }

        public static void LogAdsRewardClick()
        {
            LogEvent("ads_reward_click", null);
        }


        public static void LogAdsRewardShowSuccess()
        {
            LogFirebase("ads_reward_show_success", null);
            LogAppsflyer("af_rewarded_displayed", null);
            LogAdjust("qtfqsb", null);
        }

        public static void LogAdsRewardShowFail()
        {
            LogEvent("ads_reward_show_fail", null);
        }

        public static void LogAdsRewardComplete()
        {
            LogEvent("ads_reward_complete", null);
        }

        public static void LogAdsInterLoadFail()
        {
            LogEvent("ad_inter_load_fail", null);
        }

        public static void LogAdInterLoadSuccess()
        {
            LogEvent("ad_inter_load_success", null);
        }


        public static void LogAdInterShow()
        {
            LogFirebase("ad_inter_show", null);
            LogAppsflyer("af_inters_displayed", null);
            LogAdjust("25z8mo", null);
        }

        public static void LogAdInterClick()
        {
            LogEvent("ad_inter_click", null);
        }

        #region Only Appsflayer

        public static void LogLevelAchieved(int levelIndex, int score, bool replay)
        {
            ParameterBuilder parameterBuilder = ParameterBuilder.Create()
           .Add("af_level", levelIndex + 1)
           .Add("af_score", score)
           .Add("af_replay", replay);
            LogEvent("af_level_achieved", parameterBuilder);


            ParameterBuilder parameterBuilder1 = ParameterBuilder.Create()
         .Add("aj_level", levelIndex + 1)
         .Add("aj_score", score)
         .Add("aj_replay", replay);
            LogAdjust("6tfptm", parameterBuilder1);
        }

        public static void LogLevelFail(int levelIndex)
        {
            ParameterBuilder parameterBuilder = ParameterBuilder.Create()
            .Add("aj_level", levelIndex + 1);
            LogAdjust("retqsk", parameterBuilder);
        }

        public static void LogCallShowReward()
        {
            LogAppsflyer("af_rewarded_show", null);
            LogAdjust("41jxeh", null);
        }
        public static void LogCallShowInter()
        {
            LogAppsflyer("af_inters_show", null);
            LogAdjust("s0e7cd", null);
        }

        #endregion

        public static void LogShowAds(bool hasAds, string support)
        {
            ParameterBuilder parameterBuilder = ParameterBuilder.Create()
            .Add("has_ads", hasAds)
            .Add("support", support);
            LogEvent("show_ads", parameterBuilder);
        }

        public static void LogLevel_Unlock(int levelIndex)
        {
            ParameterBuilder parameterBuilder = ParameterBuilder.Create()
            .Add("unlock", $"level_{levelIndex + 1}_unlock");
            LogEvent("level", parameterBuilder);
        }

        public static void LogLevel_Start(int levelIndex)
        {
            ParameterBuilder parameterBuilder = ParameterBuilder.Create()
            .Add("start", $"level_{levelIndex + 1}_start");
            LogEvent("level", parameterBuilder);
        }

        public static void LogLevel_Win(int levelIndex)
        {
            ParameterBuilder parameterBuilder = ParameterBuilder.Create()
            .Add("win", $"level_{levelIndex + 1}_win");
            LogEvent("level", parameterBuilder);
        }

        public static void LogLevel_Skip(int levelIndex)
        {
            ParameterBuilder parameterBuilder = ParameterBuilder.Create()
            .Add("skip", $"level_{levelIndex + 1}_skip");
            LogEvent("level", parameterBuilder);
        }

        public static void LogReplayLevel(int levelIndex)
        {
            ParameterBuilder parameterBuilder = ParameterBuilder.Create()
            .Add("replay", $"level_{levelIndex + 1}_replay");
            LogEvent("replay_level", parameterBuilder);
        }
    }
}
