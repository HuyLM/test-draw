using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    [CreateAssetMenu(fileName = "InteractableLevelConfig", menuName = "TrickyBrain/Configs/Level/InteractableLevelConfig")]
    public class InteractableLevelConfig : LevelConfig
    {
        public string HintKey;
        public string AnswerKey;
    }
}
