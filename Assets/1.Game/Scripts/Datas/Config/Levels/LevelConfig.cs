using AtoGame.Base.UnityInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TrickyBrain
{
    public abstract class LevelConfig : ScriptableObject
    {
        [SpriteField] public Sprite Image;
        public string TitleKey;
        public AssetReferenceGameObject LevelPrefab;
    }
}
