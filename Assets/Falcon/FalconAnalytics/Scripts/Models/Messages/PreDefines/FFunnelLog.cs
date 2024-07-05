﻿using System;
using Falcon.FalconAnalytics.Scripts.Models.Attributes;
using Falcon.FalconAnalytics.Scripts.Models.Messages.Abstracts;
using Falcon.FalconCore.Scripts.Repositories;
using UnityEngine;
using UnityEngine.Scripting;

namespace Falcon.FalconAnalytics.Scripts.Models.Messages.PreDefines
{
    [Serializable]
    public class FFunnelLog : BaseFalconLog
    {
        [FSortKey] public string funnelName;
        [FSortKey] public string funnelDay;
        [FSortKey] public int priority;

        public string action;
        public int currentLevel;
        
        [Preserve]
        public FFunnelLog()
        {
        }

        public FFunnelLog(string funnelName, string action, int priority, int currentLevel = 0)
        {
            if (priority != 0 && !FDataPool.Instance.HasKey(funnelName + (priority - 1)))
                Debug.LogError(
                    $"Dwh Log invalid logic : Funnel {funnelName} not created in order in this device instance");

            FDataPool.Instance.Compute<DateTime>(funnelName + priority, (hasKey, val) =>
            {
                if (hasKey)
                    Debug.LogError(
                        $"Dwh Log invalid logic : This device already joined the funnel {funnelName} of the priority {priority}");

                return DateTime.Now.ToUniversalTime();
            });

            this.funnelName = funnelName;
            var day = FDataPool.Instance.GetOrSet(funnelName + 0, DateTime.Today.ToUniversalTime()).ToLocalTime();
            funnelDay = FTime.DateToString(day);
            this.priority = CheckNumberNonNegative(priority, nameof(priority));
            
            this.action = action;
            this.currentLevel = currentLevel;
        }

        public override string Event => "f_sdk_funnel_data";
    }
}