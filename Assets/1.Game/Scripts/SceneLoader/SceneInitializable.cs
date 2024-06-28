using AtoGame.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public abstract class SceneInitializable : MonoBehaviour
    {
        public abstract IEnumerator IInitialize();

        public abstract IEnumerator Release();
    }
}
