using AtoGame.Base.Helper;
using AtoGame.OtherModules.LocalSaveLoad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public static class GameVibration
    {
        private static bool playingClick;

        public static void Init()
        {
            Vibration.Init();
        }

        public static void PlayBackGround()
        {

        }

        public static void PlayClickAndPoint()
        {
            var saveData = LocalSaveLoadManager.Get<GameSettingSaveData>();
            if (saveData.UseHaptic == false)
            {
                return;
            }
            if (playingClick == true)
            {
                return;
            }
            Vibration.VibratePop();
            playingClick = true;
            CoroutineHelper.Start(EndOfFrame());
        }
        private static IEnumerator EndOfFrame()
        {
            yield return Yielder.EndOfFrame;
            playingClick = false;
        }

        public static void PlayCongratulations()
        {
            var saveData = LocalSaveLoadManager.Get<GameSettingSaveData>();
            if (saveData.UseHaptic == false)
            {
                return;
            }
            Vibration.VibratePeek();
        }
    }
}
