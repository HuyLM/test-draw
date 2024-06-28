using AtoGame.Base.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    [CreateAssetMenu(fileName = "IngameConfigData", menuName = "TrickyBrain/Configs/IngameConfigData")]
    public class IngameConfigData : ScriptableObject
    {
        public ShowInterstitialLevelRange[] ShowInterstitialThreholdLevelRanges;

        public int GetShowInterstitialThreshold(int levelIndex)
        {
            for(int i = 0; i < ShowInterstitialThreholdLevelRanges.Length; ++i)
            {
                if(levelIndex < ShowInterstitialThreholdLevelRanges[i].MaxLevelNumber)
                {
                    return ShowInterstitialThreholdLevelRanges[i].NextLevelThreshold;
                }
            }
            return ShowInterstitialThreholdLevelRanges[ShowInterstitialThreholdLevelRanges.Length - 1].NextLevelThreshold;
        }
    }

    [System.Serializable]
    public struct ShowInterstitialLevelRange
    {
        public int MaxLevelNumber;
        public int NextLevelThreshold;
    }
}
