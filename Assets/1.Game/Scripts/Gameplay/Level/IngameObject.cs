using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public abstract class IngameObject : MonoBehaviour
    {
        public abstract void Init();

        public abstract void End();

        public abstract void IgnoreInput(bool ignoreEnable);

        public abstract void ValidateObject();
    }
}
