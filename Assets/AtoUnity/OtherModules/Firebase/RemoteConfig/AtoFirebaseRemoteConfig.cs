using AtoGame.Base;
using Firebase.Analytics;
using Firebase.Crashlytics;
using Firebase.Extensions;
#if ATO_REMOTE_CONFIG_ENABLE
using Firebase.RemoteConfig;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace AtoGame.AtoFirebase
{
    public class AtoFirebaseRemoteConfig : Singleton<AtoFirebaseRemoteConfig>
    {
        private bool available;
        Dictionary<string, object> defaultValues = new Dictionary<string, object>();
        private Action onUpdateSuccess;
        private bool isEnableAutoFetch;
        private event Action OnReadyForUse;

        public bool IsAvailable => available;


        protected override void Initialize()
        {
            base.Initialize();
            available = false;
        }

        private void OnFirebaseInitialized(bool result, string errorMessage)
        {
            if(result == true)
            {
                OnFirebaseAvailable();
            }
            else
            {

            }
        }

        private void OnFirebaseAvailable()
        {
            if(available)
                return;
            #if ATO_REMOTE_CONFIG_ENABLE
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaultValues)
              .ContinueWithOnMainThread(task => {
                  // [END set_defaults]
                  FetchDataAsync();
              });
            #endif
        }

        public Task FetchDataAsync()
        {
#if ATO_REMOTE_CONFIG_ENABLE
            // Start a fetch request.
            // FetchAsync only fetches new data if the current data is older than the provided
            // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
            // By default the timespan is 12 hours, and for production apps, this is a good
            // number. For this example though, it's set to a timespan of zero, so that
            // changes in the console will always show up immediately.
            System.Threading.Tasks.Task fetchTask =
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
                TimeSpan.FromHours(12));
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
#else
            return null;
#endif
        }

        private void FetchComplete(Task fetchTask)
        {
            if(!fetchTask.IsCompleted)
            {
                Debug.LogError("Retrieval hasn't finished.");
                return;
            }
            #if ATO_REMOTE_CONFIG_ENABLE
            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            var info = remoteConfig.Info;
            if(info.LastFetchStatus != LastFetchStatus.Success)
            {
                Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
                return;
            }

            // Fetch successful. Parameter values must be activated to use.
            remoteConfig.ActivateAsync()
              .ContinueWithOnMainThread(
                task => {
                    Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
                    available = true;
                    OnReadyForUse?.Invoke();
                });
#endif
        }

        #region Auto Ftech
        public void AddOnUpdateValues(Action onUpdateSuccess)
        {
            this.onUpdateSuccess += onUpdateSuccess;
            EnableAutoFetch();
        }

        public void RemoveOnUpdateValues(Action onUpdateSuccess)
        {
            this.onUpdateSuccess -= onUpdateSuccess;
            DisableAutoFetch();
        }

        private void EnableAutoFetch()
        {
            if(isEnableAutoFetch)
                return;
            isEnableAutoFetch = true;
            #if ATO_REMOTE_CONFIG_ENABLE
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener
                += ConfigUpdateListenerEventHandler;
#endif
        }

        private void DisableAutoFetch()
        {
            if(isEnableAutoFetch == false)
                return;
            isEnableAutoFetch = false;
            #if ATO_REMOTE_CONFIG_ENABLE
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener
                -= ConfigUpdateListenerEventHandler;
#endif
        }
        #if ATO_REMOTE_CONFIG_ENABLE
        private void ConfigUpdateListenerEventHandler(object sender, Firebase.RemoteConfig.ConfigUpdateEventArgs args)
        {
            if(args.Error != Firebase.RemoteConfig.RemoteConfigError.None)
            {
                // DebugLog(String.Format("Error occurred while listening: {0}", args.Error));
                return;
            }
            //DebugLog(String.Format("Auto-fetch has received a new config. Updated keys: {0}",
            // string.Join(", ", args.UpdatedKeys)));
            var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
              .ContinueWithOnMainThread(task => {
                  //  DebugLog(String.Format("Remote data loaded and ready (last fetch time {0}).",
                  //info.FetchTime));
                  onUpdateSuccess?.Invoke();
              });
        }
#endif
#endregion

        public void Init()
        {
            AtoFirebaseInitializer.AddOnFirebaseInitialized(OnFirebaseInitialized);
        }

        public void AddOnReadyForUse(Action onReadyForUse)
        {
            if(available == true)
            {
                onReadyForUse?.Invoke();
            }
            else
            {
                this.OnReadyForUse += onReadyForUse;
            }
        }

        public AtoFirebaseRemoteConfig AddDefaultValue(string key, object value)
        {
            defaultValues.Add(key, value);
            return this;
        }

        public double GetFloat(string key)
        {
#if ATO_REMOTE_CONFIG_ENABLE
            return Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(key).DoubleValue;
#endif
            return 0;
        }

        public string GetString(string key)
        {
#if ATO_REMOTE_CONFIG_ENABLE
            return Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue;
#endif
            return string.Empty;
        }

        public bool GetBool(string key)
        {
#if ATO_REMOTE_CONFIG_ENABLE
            return Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(key).BooleanValue;
#endif
            return false;
        }

        public long GetLong(string key)
        {
#if ATO_REMOTE_CONFIG_ENABLE
            return Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(key).LongValue;
#endif
            return 0;
        }
    }
}
