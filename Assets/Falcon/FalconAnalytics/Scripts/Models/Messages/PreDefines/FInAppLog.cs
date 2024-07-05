using System;
using System.Globalization;
using Falcon.FalconAnalytics.Scripts.Models.Attributes;
using Falcon.FalconAnalytics.Scripts.Models.Messages.Abstracts;
using Falcon.FalconCore.Scripts.Repositories.News;
using UnityEngine;
using UnityEngine.Scripting;

namespace Falcon.FalconAnalytics.Scripts.Models.Messages.PreDefines
{
    [Serializable]
    public class FInAppLog : BaseFalconLog
    {
        [FSortKey] public string productId;
        [FSortKey] public string where;

        public string price;        
        public string currencyCode;

        public decimal localizedPrice;
        public string isoCurrencyCode;
        public string transactionId;
        public string purchaseToken;
        public int currentLevel;

        public decimal inAppLtv;
        public decimal maxInApp;
        public string ltvIsoCurrencyCode;
        
        [Preserve]
        public FInAppLog()
        {
        }

        public FInAppLog(string productId, decimal localizedPrice, string isoCurrencyCode, string where,
            string transactionId, string purchaseToken = null, int currentLevel = 0)
        {
            if (string.IsNullOrEmpty(isoCurrencyCode))
            {
                Debug.LogError(
                    "Dwh Log invalid field: Null or empty currency code of InAppLog, considering it as USD");
                isoCurrencyCode = "USD";
            }
            price = CheckNumberNonNegative(localizedPrice, nameof(price)).ToString("0.00",CultureInfo.InvariantCulture);
            currencyCode = isoCurrencyCode;
            
            this.productId = productId;
            this.where = where;
            
            this.localizedPrice = CheckNumberNonNegative(localizedPrice, nameof(localizedPrice));
            this.isoCurrencyCode = isoCurrencyCode;
            this.transactionId = transactionId;
            this.purchaseToken = purchaseToken;
            this.currentLevel = currentLevel;
            FPlayerInfoRepo.InApp.Update(localizedPrice, isoCurrencyCode);
            InAppData ltv = FPlayerInfoRepo.InApp.InAppLtv;

            inAppLtv = ltv.total;
            maxInApp = ltv.max;
            ltvIsoCurrencyCode = ltv.isoCurrencyCode;
            inAppCount = ++FPlayerInfoRepo.InApp.InAppCount;
        }

        public override string Event => "f_sdk_in_app_data";
        
    }
}