using AtoGame.Base.Helper;
using AtoGame.OtherModules.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class GameSoundManager : SoundManager<GameSoundManager>
    {
        [Header("Music")]
        [SerializeField] AudioClip gameplayBackground;

        [Header("SFX")]
        [SerializeField] AudioClip clickAndPoint;
        [SerializeField] AudioClip congratulations;
        [SerializeField] AudioClip drawSound;
        [SerializeField] AudioClip claim;
        [SerializeField] AudioClip selectPencil;

        private bool playingClickSound;

        public void PlayGameplayBackground(bool fadein = false, float fadeDuration = 1)
        {
            StopMusic();
            PlayMusic(gameplayBackground, fadein, fadeDuration);
        }

        public void PlayClickAndPoint()
        {
            if (playingClickSound == true)
            {
                return;
            }
            PlaySFX(clickAndPoint);
            playingClickSound = true;
            StartCoroutine(EndOfFrame());
        }
        private IEnumerator EndOfFrame()
        {
            yield return Yielder.EndOfFrame;
            playingClickSound = false;
        }

        public void PlayCongratulations()
        {
            PlaySFX(congratulations);
        }

        public void PlayDefaultEraser()
        {
            PlayLoopSFX(drawSound);
        }

        public void PlayClaim()
        {
            PlaySFX(claim);
        }

        public void PlaySelectPencil()
        {
            PlaySFX(selectPencil);
        }
    }
}
