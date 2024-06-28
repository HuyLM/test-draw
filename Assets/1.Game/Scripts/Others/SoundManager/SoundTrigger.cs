using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TrickyBrain
{
    public class SoundTrigger : MonoBehaviour
    {
        private void Awake()
        {
            var button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(OnSoundTrigger);
            }

        }
        public void OnSoundTrigger()
        {
            GameSoundManager.Instance.PlayClickAndPoint();
            GameVibration.PlayClickAndPoint();
        }
    }
}
