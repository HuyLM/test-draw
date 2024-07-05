using System;
using System.Collections;
using System.Collections.Generic;
using Falcon.FalconCore.Editor.Models;
using Falcon.FalconCore.Editor.Repositories;
using Falcon.FalconCore.Editor.Services;
using Falcon.FalconCore.Scripts.Repositories;
using Falcon.FalconCore.Scripts.Utils;
using Falcon.FalconCore.Scripts.Utils.Data;
using UnityEditor;

namespace Falcon.FalconCore.Editor.Views
{
    public class FalconCoreWindow : EditorWindow
    {
        [MenuItem("Falcon/FalconCore/Refresh")]
        public static void ShowWindow()
        {
            new EditorSequence(Refresh()).Start();
        }
        
        [MenuItem("Falcon/FalconCore/ClearData")]
        public static void ClearData()
        {
            new FFile(FDataPool.DataFile).Save(new Dictionary<String, String>());
        }

        private static IEnumerator Refresh()
        {
            var a = new FalconCoreInstallResponder();
            
            FPlugin plugin;
            while (!FPluginRepo.TryGet("FalconCore", out plugin))
            {
                yield return null;
            }
            yield return a.OnPluginInstalled(plugin.InstalledDirectory);
        }
    }
}