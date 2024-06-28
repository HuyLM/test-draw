using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using AtoGame.Base;
using System;

namespace TrickyBrain
{

    public class SpineAddSkinActionMono : ActionMono
    {
        [SerializeField]
        private SkeletonAnimation skeletonAnimtion;
        [SerializeField]
        private SkeletonDataAsset skeletonDataAsset;
        [SpineSkin(dataField = "skeletonDataAsset")]
        public string[] spineSkins;
        private void OnValidate()
        {
            if(skeletonAnimtion != null)
            {
                skeletonDataAsset = skeletonAnimtion.skeletonDataAsset;
            }
        }

        public override void Execute(Action onCompleted = null)
        {
            OnSetUpSkin();
            onCompleted?.Invoke();
        }
        private void OnSetUpSkin()
        {
            if(skeletonAnimtion == null)
            {
                return;
            }
            string nameNewSkin = "New Skin";
            Skin skin = new Skin(nameNewSkin);
            Skeleton skeleton = skeletonAnimtion.skeleton;
            SkeletonData skeletonData = skeleton.Data;
            int length = spineSkins.Length;
            for(int i = 0; i < length; i++)
            {
                var nameSkin = spineSkins[i];
                if(String.IsNullOrEmpty(nameSkin))
                {
                    continue;
                }
                var skinFind = skeletonData.FindSkin(nameSkin);
                if(skinFind == null)
                {
                    continue;
                }
                skin.AddSkin(skinFind);
            }
            skeleton.SetSkin(skin);
            skeleton.SetSlotsToSetupPose();
            skeletonAnimtion.Update(0);
        }

        [NaughtyAttributes.Button]
        public void Ato()
        {
            OnSetUpSkin();
        }
    }
}