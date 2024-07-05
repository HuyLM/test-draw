using System.Collections.Generic;
using Falcon.FalconAnalytics.Scripts.Models.Messages.Abstracts;
using Falcon.FalconAnalytics.Scripts.Payloads.Flex;

namespace Falcon.FalconAnalytics.Scripts.Models.Messages
{
    public class MessageBatch : List<BaseFalconLog>
    {
        public BatchWrapper Wrap()
        {
            List<DataWrapper> logWrappers = new List<DataWrapper>();
            foreach (BaseFalconLog message in this)
            {
                DataWrapper logWrapper = new DataWrapper(message);
                logWrappers.Add(logWrapper);
            }
            
            return new BatchWrapper(logWrappers);
        }
        
        public void Send()
        {
            Wrap().Send();
        }
    }
}