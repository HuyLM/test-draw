using System;
using Falcon.FalconCore.Scripts.FalconABTesting.Scripts.Model;

namespace Falcon.FalconAnalytics.Scripts.Payloads
{
    [Serializable]
    public class ActionLogConfig : FalconConfig
    {
        public bool fCoreAnalyticActionLogEnable;
    }
}