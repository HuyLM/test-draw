using System;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Falcon.FalconCore.FalconABTesting.Scripts.Payloads
{
    [Serializable]
    public class ReceiveConfigObject
    {
        public string name;
        [JsonProperty(PropertyName = "value")]
        public object Value;
        public bool abTesting;

        [Preserve]
        public ReceiveConfigObject()
        {
        }
    }
}