using UnityEngine;
using Falcon.FalconCore.Scripts.Repositories;
using Firebase.Analytics;
using Firebase.Messaging;

public class FalconFirebase : MonoBehaviour
{
    private const string FALCON_FIREBASE_TOKEN = "falcon_analytics_firebase_token";
    public static bool isInitialize = false;
    public static string firebaseToken;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        firebaseToken = FDataPool.Instance.GetOrSet(FALCON_FIREBASE_TOKEN, string.Empty);
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Set a flag here to indicate whether Firebase is ready to use by your app.
                isInitialize = true;
                Debug.Log("Firebase init success");
                SetFirebaseUserId();
                GetTokenAsync();
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    void SetFirebaseUserId()
    {
        FirebaseAnalytics.SetUserId(Falcon.FalconCore.Scripts.Repositories.News.FDeviceInfoRepo.DeviceId);
    }

    async void GetTokenAsync()
    {
        var task = FirebaseMessaging.GetTokenAsync();
        await task;
        if (task.IsCompleted)
        {
            firebaseToken = task.Result;
            FDataPool.Instance.Save(FALCON_FIREBASE_TOKEN, firebaseToken);
        }
    }
}