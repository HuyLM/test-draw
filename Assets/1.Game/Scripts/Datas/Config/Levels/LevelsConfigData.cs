using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    [CreateAssetMenu(fileName = "LevelsConfigData", menuName = "TrickyBrain/Configs/LevelsConfigData")]
    public class LevelsConfigData : ScriptableObject
    {
        public LevelConfig[] Levels;

        public LevelConfig GetLevelConfig(int index)
        {
            if(index < 0 || index >= Levels.Length)
            {
                return null;
            }
            return Levels[index];
        }
    }
}
