using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TrickyBrain
{
    public class SceneTools : MonoBehaviour
    {
        const string ProjectPath = "1.Game/";
        #region Editor Scenes Menu
        public static string LogoScenePath = "Assets/" + ProjectPath + "Scenes/LogoScene.unity";
        public static string HomeScenePath = "Assets/" + ProjectPath + "Scenes/HomeScene.unity";
        public static string GameplayScenePath = "Assets/" + ProjectPath + "Scenes/GameplayScene.unity";
        public static string LoadingScenePath = "Assets/" + ProjectPath + "Scenes/LoadingScene.unity";

        [MenuItem(ProjectPath + "Play", false, 0)]
        private static void PlayGame()
        {
            OpenLogoScene();
            EditorApplication.isPlaying = true;
        }

        [MenuItem(ProjectPath + "Scenes/Open Logo Scene", false, 1)]
        private static void OpenLogoScene()
        {
            if(UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(LogoScenePath);
            }
        }

        [MenuItem(ProjectPath + "Scenes/Open Home Scene", false, 2)]
        private static void OpenHomeScene()
        {
            if(UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(HomeScenePath);
            }
        }

        [MenuItem(ProjectPath + "Scenes/Open GamePlay Scene", false, 3)]
        private static void OpenGamePlayScene()
        {
            if(UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(GameplayScenePath);
            }
        }

        [MenuItem(ProjectPath + "Scenes/Open Loading Scene", false, 4)]
        private static void OpenLoadingScene()
        {
            if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(LoadingScenePath);
            }
        }


        #endregion
    }
}
    


