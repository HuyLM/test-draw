using AtoGame.Base;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TrickyBrain
{
    public class DataConfigs : SingletonBindAlive<DataConfigs>
    {
        public IngameConfigData IngameConfigData;


        LevelsConfigData levelsConfigData;

        public LevelsConfigData LevelsConfigData
        {
            get
            {
                if (levelsConfigData == null)
                {
                    levelsConfigData = Resources.FindObjectsOfTypeAll<LevelsConfigData>().FirstOrDefault();
                }
                return levelsConfigData;
            }
        }

        public IEnumerator LoadConfigs()
        {
            var levelsConfigLoadAsync = Resources.LoadAsync<LevelsConfigData>(typeof(LevelsConfigData).Name);
            yield return levelsConfigLoadAsync;
            levelsConfigData = levelsConfigLoadAsync.asset as LevelsConfigData;
            yield return null;
        }
    }
}
