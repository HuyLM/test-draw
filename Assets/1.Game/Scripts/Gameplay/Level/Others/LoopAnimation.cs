using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System.Runtime.CompilerServices;
using Spine;

namespace TrickyBrain
{
    public class LoopAnimation : MonoBehaviour
    {
        public SkeletonAnimation skeletonAnimation;
        public AnimationLoop[] animationLoop;
        private int loopCount = 0;
        private int currentIndex = 0;
        private void OnEnable()
        {
            skeletonAnimation.AnimationState.Complete += HandleAnimationComplete;
            PlayAnimation(0);
        }
        protected void PlayAnimation(int index)
        {
            var animLoop = animationLoop[index];
            var trackEntry = skeletonAnimation.AnimationState.SetAnimation(0, animLoop.spineAnimation, true);
        }
        protected void SetSkin(int index)
        {
            skeletonAnimation.Skeleton.SetSkin(animationLoop[index].spineSkin);
            skeletonAnimation.Skeleton.SetBonesToSetupPose();
        }
        private void HandleAnimationComplete(TrackEntry trackEntry)
        {
            loopCount++;
            if(loopCount >= animationLoop[currentIndex].loop)
            {
                currentIndex++;
                if(currentIndex >= animationLoop.Length)
                {
                    currentIndex = 0;
                }
                PlayAnimation(currentIndex);
                SetSkin(currentIndex);
                loopCount = 0;
            }
        }
        private void OnValidate()
        {
            if(animationLoop != null && animationLoop.Length > 0 && skeletonAnimation!=null)
            {
                for(int i = 0; i < animationLoop.Length; i++)
                {
                    animationLoop[i].skeletonDataAsset = this.skeletonAnimation.skeletonDataAsset;
                }
            }
        }
        private void OnDisable()
        {
            skeletonAnimation.AnimationState.ClearTracks();
            skeletonAnimation.AnimationState.Complete -= HandleAnimationComplete;

        }

        [System.Serializable]
        public class AnimationLoop
        {
            [HideInInspector]
            public SkeletonDataAsset skeletonDataAsset;
            [SpineAnimation(dataField = "skeletonDataAsset")]
            public string spineAnimation;
            [SpineSkin(dataField = "skeletonDataAsset")]
            public string spineSkin;
            public int loop;

            
        }
    }


}
