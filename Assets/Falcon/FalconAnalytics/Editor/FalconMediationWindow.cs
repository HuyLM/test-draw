using System.Collections;
using System.Threading;
using Falcon.FalconCore.Editor.Services;
using Falcon.FalconCore.Scripts.Utils.FActions.Variances.Starts;
using UnityEditor;
using UnityEngine;

namespace Falcon.FalconAnalytics.Editor
{
    public class FalconMediationWindow : EditorWindow
    {
        private const string UnityPackageExtension = ".unitypackage";

        private const string urlIronSource =
            "https://bitbucket.org/falcongame/falcon-unity-sdk/raw/63b820bc78377483ab20aa052cf81075d3256528/Assets/Falcon/Release/FalconAnalytics/FalconMediationIronSource1.1.0.unitypackage";

        private const string urlMax =
            "https://bitbucket.org/falcongame/falcon-unity-sdk/raw/63b820bc78377483ab20aa052cf81075d3256528/Assets/Falcon/Release/FalconAnalytics/FalconMediationMax1.1.0.unitypackage";

        private const string urlAdapterMax =
            "https://bitbucket.org/falcongame/falcon-unity-sdk/raw/63b820bc78377483ab20aa052cf81075d3256528/Assets/Falcon/Release/FalconAnalytics/adapter_max_6.4.2.unitypackage";

        [MenuItem("Falcon/Falcon Analytic/FalconMediation/Download FalconIronSource 1.1.0")]
        public static void DownIrs()
        {
            new EditorSequence(DownloadIrs()).Start();
        }

        public static IEnumerator DownloadIrs()
        {
            var tempFolder = Application.dataPath + "/../Temp/Ironsource" + UnityPackageExtension;
            var fileGetRequest = new FileGetRequest(urlIronSource, tempFolder);
            new Thread(fileGetRequest.Invoke).Start();
            while (!fileGetRequest.Done)
            {
                yield return null;
            }

            AssetDatabase.ImportPackage(tempFolder, true);
        }

        [MenuItem("Falcon/Falcon Analytic/FalconMediation/Download FalconMax 1.1.0")]
        public static void DownMax()
        {
            new EditorSequence(DownloadMax()).Start();
        }

        public static IEnumerator DownloadMax()
        {
            var tempFolder = Application.dataPath + "/../Temp/Max" + UnityPackageExtension;
            var fileGetRequest = new FileGetRequest(urlMax, tempFolder);
            new Thread(fileGetRequest.Invoke).Start();
            while (!fileGetRequest.Done)
            {
                yield return null;
            }

            AssetDatabase.ImportPackage(tempFolder, true);
        }

        [MenuItem("Falcon/Falcon Analytic/FalconMediation/FalconMax/Download Adapter Max 1.1.0")]
        public static void DownAdapter()
        {
            new EditorSequence(DownloadAdapter()).Start();
        }

        public static IEnumerator DownloadAdapter()
        {
            var tempFolder = Application.dataPath + "/../Temp/MaxAdapter" + UnityPackageExtension;
            var fileGetRequest = new FileGetRequest(urlAdapterMax, tempFolder);
            new Thread(fileGetRequest.Invoke).Start();
            while (!fileGetRequest.Done)
            {
                yield return null;
            }

            AssetDatabase.ImportPackage(tempFolder, true);
        }
    }
}