using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace TrickyBrain
{
    public class Point : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private SpriteRenderer sprite;
        private bool isBlockingInput;

        public SpriteRenderer Sprite => sprite;

        private void Start()
        {
#if !UNITY_EDITOR
            if(sprite != null)
            {
                sprite.enabled = false;
            }
#endif
        }

        private Line line;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetLine(Line line)
        {
            this.line = line;
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if(isBlockingInput == true)
            {
                return;
            }
            if(line == null)
            {
                return;
            }
            bool added = line.AddLine(this);
            if(added)
            {
                Hide();
            }
            else
            {
            }
        }

        public void IgnoreInput(bool ignoreInput)
        {
            isBlockingInput = ignoreInput;
        }
    }
}
