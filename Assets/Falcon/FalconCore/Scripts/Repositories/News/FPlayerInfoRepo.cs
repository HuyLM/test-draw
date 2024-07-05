using System;
using System.Collections.Generic;
using Falcon.FalconAnalytics.Scripts.Enum;
using Falcon.FalconCore.Scripts.FalconABTesting.Scripts.Model;
using Falcon.FalconCore.Scripts.Logs;
using Falcon.FalconCore.Scripts.Utils.Entities;
using UnityEngine;
using UnityEngine.Scripting;

namespace Falcon.FalconCore.Scripts.Repositories.News
{
    public static class FPlayerInfoRepo
    {
        private const string AnalyticDataPrefix = "Analytic_SDK_Data_";

        private const string AccountIDKey = AnalyticDataPrefix + "Account_ID_Key";

        private const string FirstLoginDateKey = "FIRST_DATE";
        private const string FirstLoginMillisKey = "CREATE_DATE";

        private const string MaxLevelKey = AnalyticDataPrefix + "Max_Level";
        private const string SessionIDKey = "Session_Count";
        private const string InstallVersionKey = AnalyticDataPrefix + "Install_Version";


        private static string _accountId;

        private static int? _maxPassedLevel;

        private static int? _sessionId;

        private static string _installVersion;

        static FPlayerInfoRepo()
        {
            Init();
        }

        public static DateTime FirstLogInDateTime { get; private set; }

        public static long FirstLogInMillis { get; private set; }

        public static DateTime FirstLoginDate => FirstLogInDateTime.Date;

        public static string AccountID
        {
            get =>
                _accountId ??= FDataPool.Instance.GetOrDefault(AccountIDKey, FDeviceInfoRepo.DeviceId);
            set
            {
                _accountId = value;
                FDataPool.Instance.Save(AccountIDKey, value);
            }
        }

        public static string AbTestingVariable => FalconConfig.RunningAbTesting;
        public static string AbTestingValue => FalconConfig.AbTestingString;

        public static int MaxPassedLevel
        {
            get
            {
                _maxPassedLevel ??= FDataPool.Instance.GetOrSet(MaxLevelKey, 0);

                return _maxPassedLevel.Value;
            }
            set
            {
                if (value <= MaxPassedLevel) return;
                _maxPassedLevel = value;
                FDataPool.Instance.Save(MaxLevelKey, value);
            }
        }

        public static int SessionId
        {
            get
            {
                if (_sessionId == null)
                {
                    _sessionId = FDataPool.Instance.GetOrSet(SessionIDKey, 0) + 1;
                    FDataPool.Instance.Save(SessionIDKey, _sessionId);
                }

                return _sessionId.Value;
            }
            set
            {
                _sessionId = value;
                FDataPool.Instance.Save(SessionIDKey, value);
            }
        }
        
        public static string InstallVersion
        {
            get
            {
                if (_installVersion == null)
                {
                    _installVersion = FDataPool.Instance.GetOrSet(InstallVersionKey, FDeviceInfoRepo.AppVersion);
                }

                return _installVersion;
            }
            set
            {
                _installVersion = value;
                FDataPool.Instance.Save(InstallVersionKey, value);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Init()
        {
            FirstLogInDateTime = FDataPool.Instance.GetOrSet(FirstLoginDateKey, DateTime.Now);
            FirstLogInMillis = FDataPool.Instance.GetOrSet(FirstLoginMillisKey, FTime.CurrentTimeMillis());
            CoreLogger.Instance.Info("FPlayerInfoRepo init complete");
        }

        public static class InApp
        {
            private const string InAppLtvKey = AnalyticDataPrefix + "In_App_Ltv";
            private const string InAppCountKey = AnalyticDataPrefix + "In_App_Count";

            private static int _inAppCount = FDataPool.Instance.GetOrSet(InAppCountKey, 0);

            public static InAppData InAppLtv
            {
                get
                {
                    var inAppData = new InAppData(0, 0, 0, "unknown");
                    FDataPool.Instance.Compute<Dictionary<string, InAppData>>(InAppLtvKey, (hasKey, val) =>
                    {
                        if (!hasKey) val = new Dictionary<string, InAppData>();
                        foreach (var value in val.Values) inAppData = value.count > inAppData.count ? value : inAppData;

                        return val;
                    });
                    return inAppData;
                }
            }

            public static int InAppCount
            {
                get => _inAppCount;
                set
                {
                    _inAppCount = value;
                    FDataPool.Instance.Save(InAppCountKey, value);
                }
            }

            public static void Update(decimal amount, string isoCountryCode)
            {
                FDataPool.Instance.Compute<Dictionary<string, InAppData>>(InAppLtvKey, (hasKey, val) =>
                {
                    if (!hasKey) val = new Dictionary<string, InAppData>();
                    if (!val.ContainsKey(isoCountryCode)) val[isoCountryCode] = new InAppData(0, 0, 0, isoCountryCode);
                    val[isoCountryCode].Update(amount);
                    return val;
                });
            }
        }

        public static class Ad
        {
            private const string AdLtvKey = AnalyticDataPrefix + "Ad_Ltv";

            private static readonly FConcurrentDict<AdType, int> Cache = new FConcurrentDict<AdType, int>();
            private static double? _adLtv = FDataPool.Instance.GetOrSet<double?>(AdLtvKey, null);

            public static double? AdLtv
            {
                get => _adLtv;
                set
                {
                    _adLtv = value;
                    FDataPool.Instance.Save(AdLtvKey, value);
                }
            }

            public static int AdCountOf(AdType adType)
            {
                return Cache.Compute(adType, (hasKey, val) =>
                {
                    if (!hasKey) return FDataPool.Instance.GetOrSet(KeyOf(adType), 0);
                    return val;
                });
            }

            public static void SetAdCountOf(AdType adType, int value)
            {
                Cache.Compute(adType, (hasKey, val) =>
                {
                    FDataPool.Instance.Save(KeyOf(adType), value);
                    return value;
                });
            }

            public static int IncrementAdCount(AdType adType)
            {
                return Cache.Compute(adType, (hasKey, val) =>
                {
                    return FDataPool.Instance.Compute<int>(KeyOf(adType), (hasVal, value) =>
                    {
                        if (!hasVal) value = 0;
                        value++;
                        return value;
                    });
                });
            }

            private static string KeyOf(AdType adType)
            {
                return AnalyticDataPrefix + adType + "_Ad_Count";
            }
        }
    }

    [Serializable]
    public class InAppData
    {
        public decimal total;
        public decimal max;
        public int count;
        public string isoCurrencyCode;

        [Preserve]
        public InAppData()
        {
        }

        public InAppData(decimal total, decimal max, int count, string isoCurrencyCode)
        {
            this.total = total;
            this.max = max;
            this.count = count;
            this.isoCurrencyCode = isoCurrencyCode;
        }

        public void Update(decimal amount)
        {
            total += amount;
            max = Math.Max(max, amount);
            count++;
        }
    }
}