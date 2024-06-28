using System.Collections.Generic;

namespace AtoGame.Tracking
{
    public class ParameterBuilder
    {
        private readonly Dictionary<string, object> parameters = new Dictionary<string, object>();

        public Dictionary<string, object> Params => parameters;

        public static ParameterBuilder Create()
        {
            return new ParameterBuilder();
        }

        public ParameterBuilder Add(string parameterName, object parameterValue)
        {
            if (!parameters.ContainsKey(parameterName))
            {
                parameters.Add(parameterName, parameterValue);
            }

            return this;
        }

        public Dictionary<string, string> BuildString()
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();
            foreach (var item in parameters)
            {
                temp.Add(item.Key, item.Value.ToString());
            }
            return temp;
        }

        public Dictionary<string, object> BuildObject()
        {
            Dictionary<string, object> temp = new Dictionary<string, object>();
            foreach (var item in parameters)
            {
                temp.Add(item.Key, item.Value);
            }
            return temp;
        }

#if NEWTONSOFT_ENABLE
        public Newtonsoft.Json.Linq.JObject BuildJObject()
        {
            if (parameters != null)
            {
                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();
                foreach (var d in parameters)
                {
                    jsonData.Add(d.Key, Newtonsoft.Json.Linq.JToken.FromObject(d.Value));
                }
                return jsonData;
            }
            return new Newtonsoft.Json.Linq.JObject();
        }
#endif

#if FIREBASE_ENABLE
        public Firebase.Analytics.Parameter[] BuildFirebase()
        {
            var para = new Firebase.Analytics.Parameter[parameters.Count];
            int idx = 0;
            foreach (var item in parameters)
            {
                para[idx] = new Firebase.Analytics.Parameter(item.Key, item.Value.ToString());
                idx++;
            }

            return para;
        }
#endif

#if ADJUST_ENABLE
        public com.adjust.sdk.AdjustEvent BuildAdjust(string eventName)
        {
            com.adjust.sdk.AdjustEvent adjustEvent = new com.adjust.sdk.AdjustEvent(eventName);
            foreach(var item in Params)
            {
                adjustEvent.addCallbackParameter(item.Key, item.Value.ToString());
            }
            return adjustEvent;
        }
#endif


    }
}
