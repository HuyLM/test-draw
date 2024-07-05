
using System;
using System.Collections.Generic;
using System.Threading;
using Falcon.FalconAnalytics.Scripts;
using Falcon.FalconAnalytics.Scripts.Enum;
using Falcon.FalconAnalytics.Scripts.Models.Messages;
using Falcon.FalconAnalytics.Scripts.Models.Messages.PreDefines;
using Falcon.FalconAnalytics.Scripts.Payloads.Flex;
using Falcon.FalconCore.Scripts.Services.GameObjs;
using Falcon.FalconCore.Scripts.Utils.Entities;
using Falcon.FalconCore.Scripts.Utils.Singletons;

public class BannerLogStatistic : FSingleton<BannerLogStatistic>
{
    private readonly FConcurrentDict<BannerKey, BannerValue> cache = new FConcurrentDict<BannerKey, BannerValue>();

    static BannerLogStatistic()
    {
        FGameObj.OnGameStop += (sender, args) =>
        {
            BatchWrapper wrapper = Instance.FlushCountMap();
            new Thread(() =>
            {
                try
                {
                    wrapper.Send();
                }
                catch (Exception e)
                {
                    AnalyticLogger.Instance.Warning(e.Message);
                }
            }).Start();
        };
    }

    public void Log(string adWhere, string adPrecision, string adCountry, double adRev,
        string adNetwork, string adMediation, double? adLtv = null)
    {
        BannerKey key = new BannerKey(adWhere, adPrecision, adCountry, adNetwork, adMediation);
        cache.Compute(key, (hasKey, val) =>
        {
            if (!hasKey) val = new BannerValue();
            val.Update(adRev, adLtv);
            return val;
        });
    }

    private BatchWrapper FlushCountMap()
    {
        var infos = new List<BannerKey>(cache.Keys);

        MessageBatch batch = new MessageBatch();
        foreach (var info in infos)
        {
            BannerValue value;
            if (cache.TryRemove(info, out value))
                batch.Add(new FAdLog(AdType.Banner, info.AdWhere, info.AdPrecision, info.AdCountry, value.AdRev,
                    info.AdNetwork, info.AdMediation, 0, value.AdLtv));
        }

        return batch.Wrap();
    }

    private struct BannerKey
    {
        public string AdWhere { get; }
        public string AdPrecision { get; }
        public string AdCountry { get; }
        public string AdNetwork { get; }
        public string AdMediation { get; }

        public BannerKey(string adWhere, string adPrecision, string adCountry, string adNetwork, string adMediation)
        {
            AdWhere = adWhere;
            AdPrecision = adPrecision;
            AdCountry = adCountry;
            AdNetwork = adNetwork;
            AdMediation = adMediation;
        }
    }

    private struct BannerValue
    {
        public double AdRev { get; private set; }
        public double? AdLtv { get; private set; }

        public void Update(double adRev, double? adLtv)
        {
            AdRev += adRev;
            if (adLtv != null)
            {
                AdLtv = adLtv;
            }
        }
    }
}