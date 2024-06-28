using AtoGame.Base;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class SetAnimationActionMono : ActionMono
    {
        [SerializeField] private SkeletonAnimation skeletonAnimation;
        [SerializeField] private SkeletonDataAsset skeletonDataAsset;
        [SerializeField, SpineAnimation(dataField = "skeletonDataAsset")] private string animationName;
        [SerializeField] private int trackIndex;
        [SerializeField] private bool loop;
        [SerializeField] private float timescale = 1;
        [SerializeField] private float startTime = 0;
        [NaughtyAttributes.HideIf("loop"), SerializeField, SpineEvent(dataField = "skeletonDataAsset")] private string eventName;
        [SerializeField, SpineSkin(dataField = "skeletonDataAsset")] private string skinName;

        [Header("Merge Animation")]
        [SerializeField] 
        private bool isClearFullTracks;
        [SerializeField] 
        private int[] tracksClear;



        Spine.EventData eventData;

        private void OnValidate()
        {
            if(skeletonAnimation != null)
            {
                skeletonDataAsset = skeletonAnimation.skeletonDataAsset;
            }
        }

        private Action onCompleted;
        private void Awake()
        {
            if(skeletonAnimation != null && loop == false)
            {
                if(string.IsNullOrEmpty(eventName) == false)
                {
                    eventData = skeletonAnimation.Skeleton.Data.FindEvent(eventName);
                    skeletonAnimation.AnimationState.Event += AnimationState_Event;
                }
                else
                {
                    skeletonAnimation.AnimationState.Complete += AnimationState_Complete;
                }
            }
        }


        public override void Execute(Action onCompleted = null)
        {
            if(skeletonAnimation == null)
            {
                Debug.LogError("skeletonAnimation null", this);
                return;
            }
            if(string.IsNullOrEmpty(animationName))
            {
                Debug.LogError("animationName null", this);
                return;
            }

            this.onCompleted = onCompleted;
            skeletonAnimation.Initialize(false);
            if(string.IsNullOrEmpty(skinName) == false)
            {
                skeletonAnimation.Skeleton.SetSkin(skinName);
                skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            }
            if(isClearFullTracks)
            {
                skeletonAnimation.AnimationState.ClearTracks();
            }
            else
            {
                if(tracksClear!=null && tracksClear.Length > 0)
                {
                    int length = tracksClear.Length;
                    for(int i = 0; i < length; i++)
                    {
                        skeletonAnimation.AnimationState.ClearTrack(tracksClear[i]);
                    }
                }

            }
            var trackEntry = skeletonAnimation.AnimationState.SetAnimation(trackIndex, animationName, loop);
            trackEntry.TimeScale = timescale;
            trackEntry.TrackTime = startTime;
            if(loop == true)
            {
                OnComplete(this.onCompleted);
            }
        }

        private void AnimationState_Event(Spine.TrackEntry trackEntry, Spine.Event e)
        {
            if(e.Data == eventData)
            {
                OnComplete(this.onCompleted);
            }
        }

        private void AnimationState_Complete(Spine.TrackEntry trackEntry)
        {
            OnComplete(this.onCompleted);
        }

        public override void ValidateObject()
        {
            base.ValidateObject();
            if(skeletonAnimation == null)
            {
                Debug.Log($"{name} ValidateObject: skeletonAnimation null", this);
            }

            if(skeletonDataAsset == null)
            {
                Debug.Log($"{name} ValidateObject: skeletonDataAsset null", this);
            }
        }
    }
}
