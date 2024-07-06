using AtoGame.Base.Helper;
using AtoGame.OtherModules.HUD;
using AtoGame.OtherModules.LocalSaveLoad;
using AtoGame.OtherModules.SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TrickyBrain.EventKey;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using AtoGame.Base;

namespace TrickyBrain
{
    public class DrawManager : SingletonBind<DrawManager>
    {
        [SerializeField] private Transform tfMapContainer;
        [SerializeField] private ParticleSystem[] winParticles;
        [SerializeField] private Drawer drawer;
        private AsyncOperationHandle<GameObject> operation;

        private IngameLevel _curLevel;
        private int _curLevelIndex;
        private float _startLevelTime;
        private bool isPlaying;

        public IngameLevel CurLevel => _curLevel;
        public Drawer Drawer => drawer;
        public bool IsPlaying => isPlaying;

        #region Level
        public IEnumerator IInitializeLevel(AssetReferenceGameObject arLevel, Action onComplete)
        {
            operation = arLevel.InstantiateAsync(tfMapContainer);
            yield return operation;
            _curLevel = operation.Result.GetComponent<IngameLevel>();
            onComplete?.Invoke();

        }

        public IEnumerator ReleaseLevel()
        {
            if(CurLevel != null)
            {
                Addressables.ReleaseInstance(CurLevel.gameObject);
            }
            yield return Resources.UnloadUnusedAssets();
        }

        #endregion

        private void OnDisable()
        {
            if(isPlaying == true)
            {
                float playTime = Time.realtimeSinceStartup - _startLevelTime;
                //DPTracking.LogSessionLog(playTime, "normal");
            }
        }

        public void SpawnLevel(int levelIndex, Action onSpawned)
        {
            StartCoroutine(ISpawnLevel(levelIndex, onSpawned));
        }

        public IEnumerator ISpawnLevel(int levelIndex, Action onSpawned)
        {

            _curLevelIndex = levelIndex;
            if(operation.IsValid())
            {
                yield return ReleaseLevel();
            }
            var levelConfig = DataConfigs.Instance.LevelsConfigData.GetLevelConfig(levelIndex);
            var curARLevel = levelConfig.LevelPrefab;
            yield return IInitializeLevel(curARLevel, () => {
                _startLevelTime = Time.realtimeSinceStartup;
                isPlaying = true;
                CurLevel.InitLevel(OnWonLevel, OnLosedLevel);
                onSpawned?.Invoke();
                EventDispatcher.Instance.Dispatch(new LoadedLevelEvent() { LevelIndex = levelIndex });
                StartLevel();
            });
        }

        private void OnWonLevel()
        {
            isPlaying = false;
            var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
            var levelData = saveData.GetLevel(_curLevelIndex);
            float playTime = Time.realtimeSinceStartup - _startLevelTime;
            if(levelData != null)
            {
                levelData.PlayDurantion = playTime;
                levelData.CompleteLevel();
            }
            //DPTracking.LogSessionLog(playTime, "normal");
            HUDManager.Instance.IgnoreUserInput(true);
            StartCoroutine(DelayShowWin());
        }

        private IEnumerator DelayShowWin()
        {
            // play win particles
            for(int i = 0; i < winParticles.Length; ++i)
            {
                winParticles[i].Play();
            }

            // DPVibration.PlayCongratulations();
            GameSoundManager.Instance.PlayCongratulations();
            var winPopup = PopupHUD.Instance.GetFrame<WinPopup>();
            winPopup.SetWinLevelIndex(_curLevelIndex);
            winPopup.SetWin(true);
            //winPopup.SetHighlightPosition(_curLevel.GetHighlightPosition(true));
            yield return Yielder.Wait(2);
            PopupHUD.Instance.Show<WinPopup>();
            HUDManager.Instance.IgnoreUserInput(false);
        }

        private void OnLosedLevel()
        {
            isPlaying = false;
            var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
            var levelData = saveData.GetLevel(_curLevelIndex);
            float playTime = Time.realtimeSinceStartup - _startLevelTime;
            if(levelData != null)
            {
                levelData.PlayDurantion = playTime;
                //levelData.CompleteLevel();
            }
            HUDManager.Instance.IgnoreUserInput(true);
            /*
            DPTracking.LogLevelFail(_curLevelIndex);
            DPTracking.LogLevelLog(_curLevelIndex, Falcon.FalconAnalytics.Scripts.Enum.LevelStatus.Fail, playTime);
            DPTracking.LogSessionLog(playTime, "normal");
            */
            StartCoroutine(DelayShowLose());
        }

        private IEnumerator DelayShowLose()
        {

            /*
            DPVibration.PlayCongratulations();
            DPSoundManager.Instance.PlayLose();
            */
            var winPopup = PopupHUD.Instance.GetFrame<WinPopup>();
            winPopup.SetWinLevelIndex(_curLevelIndex);
            winPopup.SetWin(false);
            //winPopup.SetHighlightPosition(_curLevel.GetHighlightPosition(false));
            yield return Yielder.Wait(1);
            PopupHUD.Instance.Show<WinPopup>();
            HUDManager.Instance.IgnoreUserInput(false);
        }

        public void StartLevel()
        {
            CurLevel.StartLevel();
        }

        public void UseHint()
        {
            CurLevel.ShowHint();
        }

        public void SkipLevel()
        {
            if(isPlaying)
            {
                var saveData = LocalSaveLoadManager.Get<LevelsSaveData>();
                if(saveData == null)
                {
                    return;
                }
                var levelData = saveData.GetLevel(_curLevelIndex);
                if(levelData != null)
                {
                    levelData.PlayDurantion = Time.realtimeSinceStartup - _startLevelTime;
                }
            }
        }
    }
}
