using Newtonsoft.Json;

namespace Falcon.FalconAnalytics.Scripts.Models.Messages.Interfaces
{
    public interface IFlexLog : IDataLog
    {
        [JsonIgnore]
        long CreatedTime { get; }
        
        string Event { get; }
    }
}