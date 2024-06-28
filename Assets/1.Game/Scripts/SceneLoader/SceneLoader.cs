using AtoGame.Base;
using AtoGame.Base.Helper;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TrickyBrain
{
    public class SceneLoader : SingletonBindAlive<SceneLoader>
    {
        private float _minTimeLoading = 0.1f;
        private int _loaderSceneIndex = 3;
        private int _homeSceneIndex = 1;
        private int _gameplaySceneIndex = 2;
        private Coroutine _sceneLoadingCoroutine;
        private int _curSceneIndex = 0;

        public void LoadHomeScene(bool stopMusic, Action<float> progress = null, Action onCompleted = null, Action onFadeInCompleted = null, Action onFadeOutStarted = null)
        {
            if(stopMusic == true)
            {
                GameSoundManager.Instance.StopMusic();
            }
            if (_sceneLoadingCoroutine != null)
            {
                StopCoroutine(_sceneLoadingCoroutine);
            }

            _sceneLoadingCoroutine = StartCoroutine(LoadAsyncScene(_homeSceneIndex, _curSceneIndex, progress, onCompleted, onFadeInCompleted, onFadeOutStarted));
        }


        public void LoadGameplayScene(bool stopMusic, Action<float> progress = null, Action onCompleted = null, Action onFadeInCompleted = null, Action onFadeOutStarted = null)
        {
            if(stopMusic == true)
            {
                GameSoundManager.Instance.StopMusic();
            }
            if (_sceneLoadingCoroutine != null)
            {
                StopCoroutine(_sceneLoadingCoroutine);
            }

            _sceneLoadingCoroutine = StartCoroutine(LoadAsyncScene(_gameplaySceneIndex, _curSceneIndex, progress, onCompleted, onFadeInCompleted, onFadeOutStarted));
        }

        private IEnumerator LoadAsyncScene(int showingSceneIndex, int hidingSceneIndex, Action<float> progress, Action onCompleted, Action onFadeInCompleted, Action onFadeOutStarted)
        {

            // play fade in scene loader
            yield return FadeIn();
            onFadeInCompleted?.Invoke();

            //+ hiện scene loader
            var loaderShowAsyncOperation = SceneManager.LoadSceneAsync(_loaderSceneIndex, LoadSceneMode.Additive);
            loaderShowAsyncOperation.allowSceneActivation = true;
            yield return loaderShowAsyncOperation;

            // SceneInitialized  Release
            var sceneRelease = FindObjectOfType<SceneInitializable>();
            if(sceneRelease != null)
            {
                yield return sceneRelease.Release();
            }

            //+ unload current scene
            var unloadAsyncOperation = SceneManager.UnloadSceneAsync(hidingSceneIndex);
            yield return unloadAsyncOperation;

            //+ load scene mới
            var loadAsyncOperation = SceneManager.LoadSceneAsync(showingSceneIndex, LoadSceneMode.Additive);
            loadAsyncOperation.allowSceneActivation = false;
            float elpased = 0f;
            while(loadAsyncOperation.isDone == false)
            {
                if(loadAsyncOperation.progress >= 0.9f)
                {
                    if(elpased < _minTimeLoading)
                    {
                        progress?.Invoke(loadAsyncOperation.progress);
                        yield return new WaitForSecondsRealtime(_minTimeLoading - elpased);
                    }
                    progress?.Invoke(1f);
                    loadAsyncOperation.allowSceneActivation = true;
                    break;
                }
                elpased += Time.deltaTime;
                progress?.Invoke(loadAsyncOperation.progress);
                yield return null;
            }
            yield return loadAsyncOperation;

            // SceneInitialized 
            var sceneInitializable = FindObjectOfType<SceneInitializable>();
            if(sceneInitializable != null)
            {
                yield return sceneInitializable.IInitialize();
            }

            yield return new WaitForSecondsRealtime(0.5f);

            // + unload scene loader
            var loaderHideAsyncOperation = SceneManager.UnloadSceneAsync(_loaderSceneIndex);
            yield return loaderHideAsyncOperation;

            //+play fade out scene loader
            yield return null;
            onFadeOutStarted?.Invoke();
            yield return FadeOut();

            _curSceneIndex = showingSceneIndex;
            onCompleted?.Invoke();
        }

        public IEnumerator FadeIn()
        {
            yield return null;
        }

        public IEnumerator FadeOut()
        {
            yield return null;
        }
    }
}
