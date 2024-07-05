using System;
using System.Diagnostics.CodeAnalysis;
using Falcon.FalconAnalytics.Scripts.Enum;
using Falcon.FalconAnalytics.Scripts.Models.Attributes;
using Falcon.FalconAnalytics.Scripts.Models.Messages.Abstracts;
using Falcon.FalconCore.Scripts.Repositories.News;
using UnityEngine.Scripting;

namespace Falcon.FalconAnalytics.Scripts.Models.Messages.PreDefines
{
    [Serializable]
    public class FAdLog : BaseFalconLog
    {
        [FSortKey] public AdType type;
        [FSortKey] public string adWhere;

        public int currentLevel;
        public string adPrecision;
        public string adCountry;
        public string adNetwork;
        public string adMediation;
        public double adRev;
        public int typeCount;

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public double? adLtv;

        [Preserve]
        public FAdLog()
        {
        }

        public FAdLog(AdType type, string adWhere, int currentLevel = 0, double? adLtv = null)
        {
            this.type = type;
            this.adWhere = adWhere;
            this.currentLevel = currentLevel;

            if (adLtv.HasValue)
            {
                this.adLtv = adLtv;
                FPlayerInfoRepo.Ad.AdLtv = adLtv.Value;
            }

            typeCount = FPlayerInfoRepo.Ad.IncrementAdCount(type);
            if (type == AdType.Interstitial || type == AdType.Reward) adCount++;
        }

        public FAdLog(AdType type, string adWhere, string adPrecision, string adCountry, double adRev,
            string adNetwork, string adMediation, int currentLevel = 0, double? adLtv = null) : this(type, adWhere,
            currentLevel, adLtv)
        {
            this.adPrecision = adPrecision;
            this.adCountry = adCountry;
            this.adRev = CheckNumberNonNegative(adRev, nameof(adRev));
            this.adNetwork = adNetwork;
            this.adMediation = adMediation;
        }

        public override string Event => "f_sdk_ads_data";

    }
}