using AtoGame.AtoFirebase;
using AtoGame.Base.Helper;
using AtoGame.Base.UI;
using AtoGame.IAP;
using AtoGame.OtherModules.Inventory;
using AtoGame.OtherModules.LocalSaveLoad;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace TrickyBrain
{
    public class StartLogoScene : MonoBehaviour
    {
        [Header("Logo UI")]
        [SerializeField] private BaseProgressBar pbLoading;
        [SerializeField] private float delayLoading = 0.3f;
        [SerializeField] private float fakeLoadingTime = 3f;

        [Header("Configs")]
        [SerializeField] private ItemDatabase _itemDataBase;
        [Header("SaveLoad")]
        [SerializeField] private LocalSaveLoadConfiguration _localSaveLoadConfiguration;


        private bool isCompletedLoading;

        private void Start()
        {
            StartCoroutine(IFakeLoading());
            StartCoroutine(ILoadConfigs());
        }

        private IEnumerator IFakeLoading()
        {
            pbLoading.gameObject.SetActive(false);
            yield return Yielder.Wait(delayLoading);
            pbLoading.gameObject.SetActive(true);
            pbLoading.Initialize();
            pbLoading.ForceFillBar(0);
            yield return null;
            float time = 0;
            while (time < fakeLoadingTime)
            {
                float progressValue = time / fakeLoadingTime;
                pbLoading.ForceFillBar(progressValue);
                time += Time.deltaTime;
                yield return null;
            }

            // wait until loading completed
            while (isCompletedLoading == false)
            {
                yield return null;
            }

            yield return Yielder.Wait(0.5f);
            pbLoading.ForceFillBar(1);
            yield return null;
            ChangeScene();
        }

        private IEnumerator ILoadConfigs()
        {
            isCompletedLoading = false;
            GameTracking.PreLoad();
            GameVibration.Init();
            InitIronSource(true);
            //Falcon.FalconGoogleUMP.FalconUMP.ShowConsentForm(InitIronSource, InitAdmob, ShowPopupATT);
            yield return DataConfigs.Instance.LoadConfigs();
            yield return Yielder.Wait(0.1f);
            SetupSaveLoad();
            GameSettingSaveData gameSettingSaveData = LocalSaveLoadManager.Get<GameSettingSaveData>();
            GameSoundManager.Instance.Setup(gameSettingSaveData);
            GameSoundManager.Instance.PlayGameplayBackground(true, 3);

            var remoteConfigSaveData = LocalSaveLoadManager.Get<RemoteConfigSaveData>();
            AtoFirebaseRemoteConfig.Instance.Preload();
            AtoFirebaseRemoteConfig.Instance.AddDefaultValue("test_key", remoteConfigSaveData.InterstitialAdCapping);
            AtoFirebaseRemoteConfig.Instance.AddOnReadyForUse(OnRemoteConfigReadyForUse);
            AtoFirebaseRemoteConfig.Instance.Init();

            yield return Yielder.Wait(0.5f);

            IapPurchaseController.Instance.Preload();
            StartIap();

            yield return Yielder.Wait(0.5f);
            isCompletedLoading = true;
        }

        #region Ads

        private void InitIronSource(bool consentValue)
        {
            IronSource.Agent.setConsent(consentValue);
            AdsManager.Instance.Preload();
        }

        private void InitAdmob()
        {
            AdsManager.Instance.InitAdmob();
        }

        private void ShowPopupATT()
        {

        }

        private void OnRemoteConfigReadyForUse()
        {
            var remoteConfigSaveData = LocalSaveLoadManager.Get<RemoteConfigSaveData>();
            float interstitialAdCapping = (float)AtoFirebaseRemoteConfig.Instance.GetFloat("test_key");
            if(Mathf.Abs(remoteConfigSaveData.InterstitialAdCapping - interstitialAdCapping) >  0.1f)
            {
                remoteConfigSaveData.InterstitialAdCapping = interstitialAdCapping;
                remoteConfigSaveData.SaveData();
            }
        }

        #endregion

        private async void StartIap()
        {
            string environment = "production";
            var options = new InitializationOptions();
            options.SetEnvironmentName(environment);

            await UnityServices.InitializeAsync(options);
            IapPurchaseController.Instance.InitializePurchasing();
        }

        private void SetupSaveLoad()
        {
            InventorySaveData itemInventorySaveData = new InventorySaveData();
            ItemInventoryController.Instance.Init(_itemDataBase, itemInventorySaveData);


            LocalSaveLoadManager.Instance.Setup(_localSaveLoadConfiguration);
            LocalSaveLoadManager.Instance.RegisterModule(typeof(InventorySaveData), itemInventorySaveData);
            LocalSaveLoadManager.Instance.RegisterModule(typeof(GameSettingSaveData));
            LocalSaveLoadManager.Instance.RegisterModule(typeof(AdsSaveData));
            LocalSaveLoadManager.Instance.RegisterModule(typeof(IngameSaveData));
            LocalSaveLoadManager.Instance.RegisterModule(typeof(LevelsSaveData));
            LocalSaveLoadManager.Instance.RegisterModule(typeof(RemoteConfigSaveData));
            LocalSaveLoadManager.Instance.Install();

        }

        private void ChangeScene()
        {
            LoadGameplayScene();
            return;
            if (AdsManager.Instance.IsAppOpenAdAvailable)
            {
                AdsManager.Instance.ShowAppOpenAd(() => {
                    LoadGameplayScene();
                });
            }
            else
            {
                LoadGameplayScene();
            }
        }

        private void LoadHomeScene()
        {
            HomeInputTransporter.Reset();
            SceneLoader.Instance.LoadHomeScene(false);
        }

        private void LoadGameplayScene()
        {
            var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
            int levelIndex = saveData.GetCurrentUnlockLevelIndex();
            if(levelIndex < 0)
            {
                levelIndex = saveData.Levels.Count - 1;
            }
            GameplayInputTransporter.Reset();
            GameplayInputTransporter.LevelIndex = levelIndex;
            SceneLoader.Instance.LoadGameplayScene(stopMusic: false, onCompleted: () => {
                //GameSoundManager.Instance.PlayGameplayBackground(true, 3);
            });
        }
    }
}
