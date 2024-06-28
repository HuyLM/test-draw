using AtoGame.Base.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class HomeSceneInitializable : SceneInitializable
    {
        public override IEnumerator IInitialize()
        {
            AdsManager.Instance.ForceShowBanner();
            yield return Yielder.WaitForMiliseconds(50);
            PanelHUD.Instance.Show<HomePanel>();
            yield break;
        }

        public override IEnumerator Release()
        {
            yield break;
        }
    }
}
