using AtoGame.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class OthersService : SingletonBindAlive<OthersService>
    {
#if UNITY_EDITOR
        public bool hasConnection;
#endif
        private InternetConectionPopup internetConnectPopup;
        

        public static bool HasInternet
        {
            get => Application.internetReachability != NetworkReachability.NotReachable;
        }

        private void Update()
        {
            bool needShowPopup = false;

#if UNITY_EDITOR
            needShowPopup = hasConnection == false;
#else
            needShowPopup = HasInternet == false;
#endif

            if(needShowPopup)
            {
                if(internetConnectPopup != null)
                {
                    if(internetConnectPopup.IsShowing)
                        return;
                }
                if(PopupHUD.HasInstance)
                    internetConnectPopup = PopupHUD.Instance.Show<InternetConectionPopup>();
            }
        }
    }
}
