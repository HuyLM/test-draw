using AtoGame.Base.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class LevelCollector : SelectableDisplayerCollector<LevelData, LevelDisplayer>
    {
        private int curLevelIndex;
        public void SetCurrentLevelIndex(int levelIndex)
        {
            curLevelIndex = levelIndex;
        }

        public override void SetupDisplayer(LevelDisplayer displayer, LevelData item)
        {
            if(displayer == null)
            {
                return;
            }
            base.SetupDisplayer(displayer, item);
            displayer.SetHighlight(curLevelIndex == item.Index);
        }
    }
}
