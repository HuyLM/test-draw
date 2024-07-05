using System.IO;
using UnityEditor;
using UnityEngine;

public class AdjustMenu : Editor
{
    [MenuItem("Falcon/Falcon Analytic/Adjust settings", false, 3)]
    public static void mediationSettings()
    {
        var adjustSettings = FalconAdjustSettingsInspector.FalconAdjustSettings;
        Selection.activeObject = adjustSettings;
    }
}