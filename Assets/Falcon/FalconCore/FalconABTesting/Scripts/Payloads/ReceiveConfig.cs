using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Falcon.FalconCore.FalconABTesting.Scripts.Payloads
{
    [Serializable]
    public class ReceiveConfig
    {
        public string runningAbTesting;
        [JsonProperty(PropertyName = "campaignMeta")]
        public Dictionary<String, Boolean> CampaignMeta = new Dictionary<String, Boolean>();
        public ReceiveConfigObject[] configs = Array.Empty<ReceiveConfigObject>();
        
        [Preserve]
        public ReceiveConfig()
        {
        }
    }
}