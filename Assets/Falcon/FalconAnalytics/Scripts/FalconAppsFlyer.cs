using System.Collections.Generic;
#if APPSFLYER_ENABLE
using AppsFlyerSDK;
#endif
using Falcon.FalconCore.Scripts.Repositories;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class FalconAppsFlyer : MonoBehaviour
    #if APPSFLYER_ENABLE
    , AppsFlyerConversionData
#endif
{
    private static string _appsflyerConversionData;

    private static string _appsflyerID;
    public static string AppsflyerID => _appsflyerID;

    private static string _appsflyerAdgroupID;
    private static string _appsflyerOrigCost;
    private static string _appsflyerAfCostCurrency;
    private static bool _appsflyerIsFirstLaunch;
    private static string _appsflyerCampaignID;
    private static string _appsflyerAfCid;
    private static string _appsflyerMediaSource;
    private static string _appsflyerAdvertisingID;
    private static string _appsflyerAfStatus;
    private static string _appsflyerCostCentsUsd;
    private static string _appsflyerAfCostValue;
    private static string _appsflyerAfCostModel;
    private static string _appsflyerAfAD;
    private static bool _appsflyerIsRetargeting;
    private static string _appsflyerAdgroup;

    public static string AppsflyerAdgroupID => _appsflyerAdgroupID;
    public static string AppsflyerOrigCost => _appsflyerOrigCost;
    public static string AppsflyerAfCostCurrency => _appsflyerAfCostCurrency;
    public static bool AppsflyerIsFirstLaunch => _appsflyerIsFirstLaunch;
    public static string AppsflyerCampaignID => _appsflyerCampaignID;
    public static string AppsflyerAfCid => _appsflyerAfCid;
    public static string AppsflyerMediaSource => _appsflyerMediaSource;
    public static string AppsflyerAdvertisingID = _appsflyerAdvertisingID;
    public static string AppsflyerAfStatus => _appsflyerAfStatus;
    public static string AppsflyerCostCentsUsd => _appsflyerCostCentsUsd;
    public static string AppsflyerAfCostValue => _appsflyerAfCostValue;
    public static string AppsflyerAfCostModel => _appsflyerAfCostModel;
    public static string AppsflyerAfAD => _appsflyerAfAD;
    public static bool AppsflyerIsRetargeting => _appsflyerIsRetargeting;
    public static string AppsflyerAdgroup => _appsflyerAdgroup;

    private const string FALCON_APPSFLYER_CONVERSION_DATA = "falcon_analytics_appsflyer_conversion_data";

    void UpdateConvrersionData()
    {
        if (_appsflyerConversionData.Length == 0) return;
        IDictionary<string, JToken> jsonObject = JObject.Parse(_appsflyerConversionData);
        if (jsonObject.ContainsKey("adgroup_id"))
        {
            _appsflyerAdgroupID = jsonObject["adgroup_id"].ToString();
        }

        if (jsonObject.ContainsKey("orig_cost"))
        {
            _appsflyerOrigCost = jsonObject["orig_cost"].ToString();
        }

        if (jsonObject.ContainsKey("af_cost_currency"))
        {
            _appsflyerAfCostCurrency = jsonObject["af_cost_currency"].ToString();
        }

        if (jsonObject.ContainsKey("is_first_launch"))
        {
            bool.TryParse(jsonObject["is_first_launch"].ToString(), out _appsflyerIsFirstLaunch);
        }

        if (jsonObject.ContainsKey("campaign_id"))
        {
            _appsflyerCampaignID = jsonObject["campaign_id"].ToString();
        }

        if (jsonObject.ContainsKey("af_c_id"))
        {
            _appsflyerAfCid = jsonObject["af_c_id"].ToString();
        }

        if (jsonObject.ContainsKey("media_source"))
        {
            _appsflyerMediaSource = jsonObject["media_source"].ToString();
        }

        if (jsonObject.ContainsKey("advertising_id"))
        {
            _appsflyerAdvertisingID = jsonObject["advertising_id"].ToString();
        }

        if (jsonObject.ContainsKey("af_status"))
        {
            _appsflyerAfStatus = jsonObject["af_status"].ToString();
        }

        if (jsonObject.ContainsKey("cost_cents_USD"))
        {
            _appsflyerCostCentsUsd = jsonObject["cost_cents_USD"].ToString();
        }

        if (jsonObject.ContainsKey("af_cost_value"))
        {
            _appsflyerAfCostValue = jsonObject["af_cost_value"].ToString();
        }

        if (jsonObject.ContainsKey("af_cost_model"))
        {
            _appsflyerAfCostModel = jsonObject["af_cost_model"].ToString();
        }

        if (jsonObject.ContainsKey("af_ad"))
        {
            _appsflyerAfAD = jsonObject["af_ad"].ToString();
        }

        if (jsonObject.ContainsKey("is_retargeting"))
        {
            bool.TryParse(jsonObject["is_retargeting"].ToString(), out _appsflyerIsRetargeting);
        }

        if (jsonObject.ContainsKey("adgroup"))
        {
            _appsflyerAdgroup = jsonObject["adgroup"].ToString();
        }
    }

    void Awake()
    {
        #if APPSFLYER_ENABLE
        DontDestroyOnLoad(gameObject);
        _appsflyerConversionData = FDataPool.Instance.GetOrSet(FALCON_APPSFLYER_CONVERSION_DATA, string.Empty);
        UpdateConvrersionData();
        var appsFlyerSettings = Resources.Load<AppsFlyerSettings>("AppsFlyerSettings");
        AppsFlyer.setCustomerUserId(Falcon.FalconCore.Scripts.Repositories.News.FDeviceInfoRepo.DeviceId);
        AppsFlyer.initSDK(appsFlyerSettings.devKey, appsFlyerSettings.appID, this);
#if UNITY_IOS && !UNITY_EDITOR
        AppsFlyer.waitForATTUserAuthorizationWithTimeoutInterval(60);
#endif
        AppsFlyer.startSDK();
        AppsFlyerAdRevenue.start();
        _appsflyerID = AppsFlyer.getAppsFlyerId();
        Debug.Log("------ appsflyerID : " + _appsflyerID);
        #endif
    }

    public void onConversionDataSuccess(string conversionData)
    {
        Debug.Log("------conversionData : " + conversionData);
        _appsflyerConversionData = conversionData;
        FDataPool.Instance.Save(FALCON_APPSFLYER_CONVERSION_DATA, _appsflyerConversionData);
        UpdateConvrersionData();
        //AppsFlyer.AFLog("onConversionDataSuccess", conversionData);
        //Dictionary<string, object> conversionDataDictionary = AppsFlyer.CallbackStringToDictionary(conversionData);
    }

    public void onConversionDataFail(string error)
    {
        //AppsFlyer.AFLog("onConversionDataFail", error);
        Debug.Log("------onConversionDataFail : " + error);
    }

    public void onAppOpenAttribution(string attributionData)
    {
        //AppsFlyer.AFLog("onAppOpenAttribution: This method was replaced by UDL. This is a fake call.", attributionData);
        Debug.Log("------onAppOpenAttribution : " + attributionData);
    }

    public void onAppOpenAttributionFailure(string error)
    {
        //AppsFlyer.AFLog("onAppOpenAttributionFailure: This method was replaced by UDL. This is a fake call.", error);
        Debug.Log("------onAppOpenAttributionFailure : " + error);
    }
}