using System;
using System.Collections.Generic;
using Falcon.FalconAnalytics.Scripts.Payloads.Flex;
using Falcon.FalconCore.Scripts.Repositories;
using Falcon.FalconCore.Scripts.Services.GameObjs;
using Falcon.FalconCore.Scripts.Utils;
using Falcon.FalconCore.Scripts.Utils.Entities;
using Falcon.FalconCore.Scripts.Utils.FActions.Variances.Ends;
using Falcon.FalconCore.Scripts.Utils.Singletons;

namespace Falcon.FalconAnalytics.Scripts.Services
{
    public class LogSendService : FSingleton<LogSendService>
    {
        private const string CachedRequestList = "Request_Queue";

        private readonly RepeatAction flushing;

        private readonly FLimitQueue<DataWrapper> waitingQueue = new FLimitQueue<DataWrapper>(100);

        public LogSendService()
        {
            flushing = new RepeatAction(FlushQueue, TimeSpan.FromSeconds(15));
            flushing.Schedule();
            LoadRequests();
            FGameObj.OnGameStop += (a, b) => SaveRequest();
            FGameObj.OnGameContinue += (a, b) => LoadRequests();
        }

        private void FlushQueue()
        {
            if (waitingQueue.Count > 0)
                AnalyticLogger.Instance.Info(waitingQueue.Count + " requests is waiting in the main execute queue");

            var dataWrappers = new List<DataWrapper>(waitingQueue);

            if(dataWrappers.Count == 0) return;
            try
            {
                new BatchWrapper(dataWrappers).Send();
                for (var i = 0; i < dataWrappers.Count; i++)
                {
                    waitingQueue.TryDequeue(out _);
                }
            }
            catch (Exception e)
            {
                AnalyticLogger.Instance.Error(e);
            }
        }

        private void SaveRequest()
        {
            var unsentData = waitingQueue.Clear();
            FDataPool.Instance.Save(CachedRequestList, unsentData);

            AnalyticLogger.Instance.Info("Unsent requests Save success : " + unsentData.Count + " requests");
        }

        private void LoadRequests()
        {
            FDataPool.Instance.ComputeIfPresent<List<DataWrapper>>(CachedRequestList, wrappers =>
            {
                if (wrappers != null)
                {
                    waitingQueue.EnqueueAll(wrappers);
                    AnalyticLogger.Instance.Info("Unsent requests Load success : " + wrappers.Count + " requests");
                }

                return null;
            });
        }

        public void Enqueue(DataWrapper wrapper)
        {
            AnalyticLogger.Instance.Info("Sending message: " + wrapper.GetType().Name + JsonUtil.ToJson(wrapper));
            waitingQueue.Enqueue(wrapper);
        }
    }
}