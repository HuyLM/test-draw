using System.IO;
using UnityEditor;

public class FalconAdjustSettingsInspector : Editor
{
    private static FalconAdjustSettings falconAdjustSettings;
    public static string PATH_ASSET = "Assets/Adjust/Resources/FalconAdjustSettings.asset";
    public static FalconAdjustSettings FalconAdjustSettings
    {
        get
        {
            if (falconAdjustSettings == null)
            {
                falconAdjustSettings = AssetDatabase.LoadAssetAtPath<FalconAdjustSettings>(PATH_ASSET);
                if (falconAdjustSettings == null)
                {
                    FalconAdjustSettings asset = CreateInstance<FalconAdjustSettings>();
                    Directory.CreateDirectory("Assets/Adjust/Resources");
                    AssetDatabase.CreateAsset(asset, PATH_ASSET);
                    falconAdjustSettings = asset;
                }
            }
            return falconAdjustSettings;
        }
    }
}