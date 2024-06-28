using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TrickyBrain
{
    public class GameplaySceneInitializable : SceneInitializable
    {

        public override IEnumerator IInitialize()
        {
            int levelIndex = GameplayInputTransporter.LevelIndex;
            yield return DrawManager.Instance.SpawnLevel(levelIndex, ()=> {
                PopupHUD.Instance.Show<HomePopup>();
            });
            yield break;
        }

        public override IEnumerator Release()
        {
            yield return DrawManager.Instance.ReleaseLevel();
            yield break;
        }
    }
}
