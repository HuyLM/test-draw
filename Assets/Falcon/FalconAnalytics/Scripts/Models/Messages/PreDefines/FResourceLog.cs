using System;
using Falcon.FalconAnalytics.Scripts.Enum;
using Falcon.FalconAnalytics.Scripts.Models.Attributes;
using Falcon.FalconAnalytics.Scripts.Models.Messages.Abstracts;
using UnityEngine.Scripting;

namespace Falcon.FalconAnalytics.Scripts.Models.Messages.PreDefines
{
    [Serializable]
    public class FResourceLog : BaseFalconLog
    {
        [FSortKey] public FlowType flowType;
        [FSortKey] public string itemType;
        [FSortKey] public string currency;

        public string itemId;
        public long amount;
        public int currentLevel;

        [Preserve]
        public FResourceLog()
        {
        }

        public FResourceLog(FlowType flowType, string itemType, string currency, string itemId, long amount,
            int currentLevel = 0)
        {
            this.flowType = flowType;
            this.itemType = itemType;
            this.currency = currency;

            this.itemId = itemId;
            this.amount = CheckNumberNonNegative(amount, nameof(amount));
            this.currentLevel = currentLevel;
        }

        public override string Event => "f_sdk_resource_data";
        
    }
}