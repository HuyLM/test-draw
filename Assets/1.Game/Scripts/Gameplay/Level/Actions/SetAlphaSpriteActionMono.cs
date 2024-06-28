using AtoGame.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class SetAlphaSpriteActionMono : ActionMono
    {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField, Range(0, 1)] private float alpha;
        public override void Execute(Action onCompleted = null)
        {
            Color color = sprite.color;
            color.a = alpha;
            sprite.color = color;
            OnComplete(onCompleted);
        }

        public override void ValidateObject()
        {
            base.ValidateObject();
            if(sprite == null)
            {
                Debug.Log($"{name} ValidateObject: sprite null", this);
            }
        }

    }
}
