using System;
using Falcon.FalconCore.FalconABTesting.Scripts.Payloads;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Falcon.FalconCore.FalconABTesting.Scripts.Models
{
    [Serializable]
    public class ConfigObject
    {
        public string name;
        [JsonProperty(PropertyName = "value")] 
        public object Value;

        [Preserve]
        public ConfigObject()
        {
        }

        public ConfigObject(string name, object value)
        {
            this.name = name;
            Value = value;
        }

        public ConfigObject(ReceiveConfigObject receiveConfigObject)
        {
            name = receiveConfigObject.name;
            Value = receiveConfigObject.Value;
        }
    }
}