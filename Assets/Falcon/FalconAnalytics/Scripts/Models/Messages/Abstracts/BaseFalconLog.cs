using Falcon.FalconAnalytics.Scripts.Models.Messages.Interfaces;
using Falcon.FalconAnalytics.Scripts.Payloads.Flex;
using Falcon.FalconAnalytics.Scripts.Services;
using Newtonsoft.Json;

namespace Falcon.FalconAnalytics.Scripts.Models.Messages.Abstracts
{
    public abstract class BaseFalconLog : BasicLog, IFlexLog
    {

        [JsonIgnore] public long CreatedTime => clientCreateDate;

        [JsonIgnore] public virtual string Event => GetType().Name;

        public virtual void Send()
        {
            LogSendService.Instance.Enqueue(new DataWrapper(this));
        }

        #region Check Params

        protected int CheckNumberNonNegative(int i, string fieldName)
        {
            if (i < 0)
            {
                AnalyticLogger.Instance.Error(
                    $"Dwh Log invalid field: the value of field {fieldName} of {GetType().Name.Substring(3)} must be non-negative, input value '{i}'");
                return 0;
            }

            return i;
        }

        protected long CheckNumberNonNegative(long i, string fieldName)
        {
            if (i < 0)
            {
                AnalyticLogger.Instance.Error(
                    $"Dwh Log invalid field: the value of field {fieldName} of {GetType().Name.Substring(3)} must be non-negative, input value '{i}'");
                return 0;
            }

            return i;
        }

        protected float CheckNumberNonNegative(float i, string fieldName)
        {
            if (i < 0)
            {
                AnalyticLogger.Instance.Error(
                    $"Dwh Log invalid field: the value of field {fieldName} of {GetType().Name.Substring(3)} must be non-negative, input value '{i}'");
                return 0;
            }

            return i;
        }

        protected double CheckNumberNonNegative(double i, string fieldName)
        {
            if (i < 0)
            {
                AnalyticLogger.Instance.Error(
                    $"Dwh Log invalid field: the value of field {fieldName} of {GetType().Name.Substring(3)} must be non-negative, input value '{i}'");
                return 0;
            }

            return i;
        }

        protected decimal CheckNumberNonNegative(decimal i, string fieldName)
        {
            if (i < 0)
            {
                AnalyticLogger.Instance.Error(
                    $"Dwh Log invalid field: the value of field {fieldName} of {GetType().Name.Substring(3)} must be non-negative, input value '{i}'");
                return 0;
            }

            return i;
        }

        #endregion
    }
}