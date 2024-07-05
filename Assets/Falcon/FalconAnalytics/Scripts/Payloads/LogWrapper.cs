using System;
using System.Linq;
using System.Net.Http;
using Falcon.FalconCore.Scripts.Utils;
using Falcon.FalconCore.Scripts.Utils.FActions.Variances.Starts;
using UnityEngine.Scripting;

namespace Falcon.FalconAnalytics.Scripts.Payloads
{
    [Serializable]
    public abstract class LogWrapper
    {
        public string data;

        public abstract string URL { get; }
        
        [Preserve]
        protected LogWrapper(string data)
        {
            this.data = data;
        }

        public virtual void Send()
        {
            var request = new HttpRequest
            {
                RequestType = HttpMethod.Post,
                URL = URL,
                JsonBody = JsonUtil.ToJson(this),
                Timeout = TimeSpan.FromSeconds(60)
            };
            request.Invoke();
            if (string.IsNullOrEmpty(request.Result))
            {
                throw request.Exception;
            }
            ValidateResponse(new string(request.Result.Where(c => !char.IsControl(c)).ToArray()));
        }

        protected abstract void ValidateResponse(string response);
    }
}