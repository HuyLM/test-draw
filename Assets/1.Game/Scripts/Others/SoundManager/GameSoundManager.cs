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
        [SerializeField] AudioClip homeBackground;
        [SerializeField] AudioClip gameplayBackground;

        [Header("SFX")]
        [SerializeField] AudioClip clickAndPoint;
        [SerializeField] AudioClip congratulations;
        [SerializeField] AudioClip laughTroll;
        [SerializeField] AudioClip eraser;
        [SerializeField] AudioClip lose;
        [SerializeField] AudioClip correct;
        [SerializeField] AudioClip claim;

        private bool playingClickSound;

        public void PlayHomeBackground(bool fadein = false, float fadeDuration = 1)
        {
            StopMusic();
            PlayMusic(homeBackground, fadein, fadeDuration);
        }

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

        public void PlayLaughTroll()
        {
            PlaySFX(laughTroll);
        }

        public void PlayDefaultEraser()
        {
            PlayLoopSFX(eraser);
        }

        public void PlayLose()
        {
            PlaySFX(lose);
        }

        public void PlayClaim()
        {
            PlaySFX(claim);
        }

        public void PlayCorrect()
        {
            PlaySFX(correct);
        }
    }
}
