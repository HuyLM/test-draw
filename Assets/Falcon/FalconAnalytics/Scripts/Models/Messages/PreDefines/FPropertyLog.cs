using System;
using Falcon.FalconAnalytics.Scripts.Models.Attributes;
using Falcon.FalconAnalytics.Scripts.Models.Messages.Abstracts;
using UnityEngine.Scripting;

namespace Falcon.FalconAnalytics.Scripts.Models.Messages.PreDefines
{
    [Serializable]
    public class FPropertyLog : BaseFalconLog
    {
        [FSortKey] public string pName;
        [FSortKey] public string pValue;

        public int priority;
        public int currentLevel;

        [Preserve]
        public FPropertyLog()
        {
        }

        public FPropertyLog(string pName, string pValue, int priority, int currentLevel = 0)
        {
            this.pName = pName;
            this.pValue = pValue;
            
            this.priority = CheckNumberNonNegative(priority, nameof(priority));
            this.currentLevel = currentLevel;
        }

        public override string Event => "f_sdk_property_data";
        
    }
}