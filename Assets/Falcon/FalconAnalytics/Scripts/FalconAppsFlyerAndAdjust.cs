using System.Collections.Generic;
#if USE_APPSFLYER
using AppsFlyerSDK;
#elif USE_ADJUST
using com.adjust.sdk;
#endif
using Falcon.FalconCore.Scripts.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

#if USE_APPSFLYER
public class FalconAppsFlyerAndAdjust : MonoBehaviour, IAppsFlyerConversionData
#elif USE_ADJUST
public class FalconAppsFlyerAndAdjust : MonoBehaviour
#endif
{
    private static string _appsflyerConversionData;
    private static string _appsflyerID;
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
    public static string AppsflyerID => _appsflyerID;
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
    private static string _adjustConversionData;
    private static string _adjustID;
    private static string _adjustTrackerToken;
    private static string _adjustNetwork;
    private static string _adjustCampaign;
    private static string _adjustAdGroup;
    private static string _adjustCreative;
    private static string _adjustCostType;
    private static string _adjustCostAmount;
    private static string _adjustCostCurrency;
    public static string AdjustID => _adjustID;
    public static string AdjustTrackerToken => _adjustTrackerToken;
    public static string AdjustNetwork => _adjustNetwork;
    public static string AdjustCampaign => _adjustCampaign;
    public static string AdjustAdGroup => _adjustAdGroup;
    public static string AdjustCreative => _adjustCreative;
    public static string AdjustCostType => _adjustCostType;
    public static string AdjustCostAmount => _adjustCostAmount;
    public static string AdjustCostCurrency => _adjustCostCurrency;
    private const string FALCON_ADJUST_CONVERSION_DATA = "falcon_analytics_adjust_conversion_data";

    static void UpdateConvrersionData()
    {
#if USE_APPSFLYER
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
#elif USE_ADJUST
        if (_adjustConversionData.Length == 0) return;

        IDictionary<string, JToken> jsonObject = JObject.Parse(_adjustConversionData);
        if (jsonObject.ContainsKey(AdjustUtils.KeyTrackerToken))
        {
            _adjustTrackerToken = jsonObject[AdjustUtils.KeyTrackerToken].ToString();
        }

        if (jsonObject.ContainsKey(AdjustUtils.KeyNetwork))
        {
            _adjustNetwork = jsonObject[AdjustUtils.KeyNetwork].ToString();
        }

        if (jsonObject.ContainsKey(AdjustUtils.KeyCampaign))
        {
            _adjustCampaign = jsonObject[AdjustUtils.KeyCampaign].ToString();
        }

        if (jsonObject.ContainsKey(AdjustUtils.KeyAdgroup))
        {
            _adjustAdGroup = jsonObject[AdjustUtils.KeyAdgroup].ToString();
        }

        if (jsonObject.ContainsKey(AdjustUtils.KeyCreative))
        {
            _adjustCreative = jsonObject[AdjustUtils.KeyCreative].ToString();
        }

        if (jsonObject.ContainsKey(AdjustUtils.KeyCostType))
        {
            _adjustCostType = jsonObject[AdjustUtils.KeyCostType].ToString();
        }

        if (jsonObject.ContainsKey(AdjustUtils.KeyCostAmount))
        {
            _adjustCostAmount = jsonObject[AdjustUtils.KeyCostAmount].ToString();
        }

        if (jsonObject.ContainsKey(AdjustUtils.KeyCostCurrency))
        {
            _adjustCostCurrency = jsonObject[AdjustUtils.KeyCostCurrency].ToString();
        }
#endif
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
#if USE_APPSFLYER
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
#elif USE_ADJUST
        /*
        _adjustConversionData = FDataPool.Instance.GetOrSet(FALCON_ADJUST_CONVERSION_DATA, string.Empty);
        UpdateConvrersionData();
        var adjustSettings = Resources.Load<FalconAdjustSettings>("FalconAdjustSettings");
        AdjustConfig config = new AdjustConfig(adjustSettings.appToken, AdjustEnvironment.Production, true);
        config.setLogLevel(AdjustLogLevel.Suppress);
        config.setSendInBackground(true);
        config.setAttributionChangedDelegate(OnAttributionChanged);
        Adjust.start(config);
        _adjustID = Adjust.getAdid();
        */
#endif
    }

    public static void PreStart()
    {
        _adjustConversionData = FDataPool.Instance.GetOrSet(FALCON_ADJUST_CONVERSION_DATA, string.Empty);
        UpdateConvrersionData();
    }

    public static void PostStart()
    {
        _adjustID = Adjust.getAdid();
    }

    public static void OnAttributionChanged(AdjustAttribution data)
    {
        AdjustAttributionExtends aae = (AdjustAttributionExtends)data;
        _adjustConversionData = JsonUtility.ToJson(aae);
        Debug.Log("------conversionData : " + _adjustConversionData);
        FDataPool.Instance.Save(FALCON_ADJUST_CONVERSION_DATA, _adjustConversionData);
        UpdateConvrersionData();
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