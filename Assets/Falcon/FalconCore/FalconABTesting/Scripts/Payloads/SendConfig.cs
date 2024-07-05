using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Falcon.FalconAnalytics.Scripts.Enum;
using Falcon.FalconCore.FalconABTesting.Scripts.Models;
using Falcon.FalconCore.FalconABTesting.Scripts.Repositories;
using Falcon.FalconCore.Scripts.FalconABTesting.Scripts.Model;
using Falcon.FalconCore.Scripts.Logs;
using Falcon.FalconCore.Scripts.Repositories.News;
using Falcon.FalconCore.Scripts.Utils;
using Falcon.FalconCore.Scripts.Utils.FActions.Variances.Starts;
using Newtonsoft.Json;

namespace Falcon.FalconCore.FalconABTesting.Scripts.Payloads
{
    [Serializable]
    public class SendConfig
    {
#pragma warning disable S1075 // URIs should not be hardcoded    
        [JsonIgnore] private const string ServerURL = "https://gateway.data4game.com/kapigateway/abtestingservice/rmconfigs/getRemoteConfigsByFilters";
#pragma warning restore S1075 // URIs should not be hardcoded
        
        public long createdDate = FPlayerInfoRepo.FirstLogInMillis / 1000;//millis second -> second

        [JsonProperty(PropertyName = "runningABTesting")]
        public string runningAbTesting = FalconConfig.RunningAbTesting;

        public ConfigObject[] properties;

        public ConfigObject[] abTestingConfigs = FalconConfigRepo.TestingConfigs.ToArray();
        
        [JsonProperty(PropertyName = "campaignMeta")]
        public Dictionary<String, Boolean> CampaignMeta = FalconConfigRepo.CampaignMeta;
        
        public SendConfig()
        {
            properties = new[]
            {
                new ConfigObject("deviceName", FDeviceInfoRepo.DeviceName),
                new ConfigObject("numberOfVideos", (FPlayerInfoRepo.Ad.AdCountOf(AdType.Interstitial) + FPlayerInfoRepo.Ad.AdCountOf(AdType.Reward)).ToString()),
                new ConfigObject("platform", FDeviceInfoRepo.Platform),
                // ReSharper disable once StringLiteralTypo
                new ConfigObject("appversion",FDeviceInfoRepo.AppVersion),
                new ConfigObject("level", FPlayerInfoRepo.MaxPassedLevel.ToString())
            };
        }

        public ReceiveConfig Connect()
        {
            CoreLogger.Instance.Info(JsonUtil.ToJson(this));
            string response = new HttpRequest
            {
                RequestType = HttpMethod.Post,
                URL = ServerURL,
                JsonBody = JsonUtil.ToJson(this),
                Headers = new Dictionary<string, string> { { "Game-Id", FDeviceInfoRepo.PackageName } }
            }.InvokeAndGet();
            
            CoreLogger.Instance.Info(response);
            return JsonUtil.FromJson<ReceiveConfig>(response);
        }
    }
}