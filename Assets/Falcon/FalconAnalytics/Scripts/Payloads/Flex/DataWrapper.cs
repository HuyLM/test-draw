using System;
using Falcon.FalconAnalytics.Scripts.Models.Messages.Interfaces;
using Falcon.FalconAnalytics.Scripts.Services;
using Falcon.FalconCore.Scripts.Repositories.News;
using Falcon.FalconCore.Scripts.Utils;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;

namespace Falcon.FalconAnalytics.Scripts.Payloads.Flex
{
    [Serializable]
    public class DataWrapper : LogWrapper
    {
        public long clientSendTime;
        public string @event;
        public string packageName = FDeviceInfoRepo.PackageName;
        public string platform = FDeviceInfoRepo.Platform;

        [Preserve]
        [JsonConstructor]
        public DataWrapper(string messageType, string data, long clientSendTime, string @event) : base(data)
        {
            this.clientSendTime = clientSendTime;
            this.@event = @event;
        }

        [Preserve]
        public DataWrapper(IFlexLog message) : base(JsonUtil.ToJson(message, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ContractResolver = new ConvertContractResolver(message.GetType())
        }))
        {
            clientSendTime = message.CreatedTime;
            @event = message.Event;
        }

        public override string URL => "https://dwhapi-v2.data4game.com/event-log-v2";

        protected override void ValidateResponse(string response)
        {
            if (string.Equals(response, "Request processed successfully.", StringComparison.Ordinal) ||
                string.Equals(response, "Request processed successfully.\n", StringComparison.Ordinal))
                AnalyticLogger.Instance.Info(@event + " has been sent successfully");
            else
                Debug.LogError(@event +
                               " has been sent failed with the response of: " + response);
        }
    }
}