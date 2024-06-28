using AtoGame.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public partial class EventKey
    {

        // Gameplay
        public struct LoadedLevelEvent : IEventParams
        {
            public int LevelIndex;
        }

        public struct StartLevelEvent : IEventParams
        {
            public float PlayTime;
        }

        public struct EndLevelEvent : IEventParams
        {
            public bool IsWin;
        }

        public struct MoveCameraTo : IEventParams
        {
            public Vector2 Position;
        }

        public struct IgnoreInputEvent : IEventParams
        {
            public bool EnableIgnore;
        }

        public struct InitFindableLevelEvent : IEventParams
        {
            public int NeedFindableObjectNumber;
        }

        public class AddFindableObjectsEvent : IEventParams
        {
            public FindableObject[] FindableObjects;
        }
        
        public class InitUseableObjectsEvent : IEventParams
        {
            public UseableObject[] UseableObjects;
        }

        public class AddUseableObjectsEvent : IEventParams
        {
            public UseableObject UseableObject;
        }

        public class RemoveUseableObjectsEvent : IEventParams
        {
            public UseableObject[] UseableObjects;
        }

        public class ReplaceUseableObjectEvent : IEventParams
        {
            public UseableObject From;
            public UseableObject To;
        }
    }
}
