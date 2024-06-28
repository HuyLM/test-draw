using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TrickyBrain
{
    public class NativeAdObject : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image imgNativeAd;
        [SerializeField] private GameObject goAdChoicesLogo;
        [SerializeField] private GameObject goAdLoading;
        [SerializeField] private bool showNativeIcon;
        [SerializeField] private bool onlyShowWhenReady;
        [SerializeField] private GameObject goNativeAd;
        /* [Remove-Admob]
        private void OnEnable()
        {
            goAdLoading.SetActive(true);
            imgNativeAd.gameObject.SetActive(false);
            ShowNativeAd();
            AdsManager.Instance.AddOnNativeAdLoadSuccess(OnNativeAdLoadSuccess);
            if(canvas != null)
            {
                canvas.worldCamera = GameplayCamera.Instance.GetCamera();
                ;
            }
            if(onlyShowWhenReady == true)
            {
                goNativeAd.SetActive(false);
            }
        }

        private void OnDisable()
        {
            AdsManager.Instance.RemoveOnNativeAdLoadSuccess(OnNativeAdLoadSuccess);
        }

        private void OnDestroy()
        {
            AdsManager.Instance.NativeAd.Destroy();
        }

        private void OnNativeAdLoadSuccess()
        {
            ShowNativeAd();
        }

        private void ShowNativeAd()
        {
            if(AdsManager.Instance.NativeAd.IsAvailable)
            {
                if(showNativeIcon)
                {
                    bool success = ShowNativeIcon();
                    if(success == false)
                    {
                        ShowNativeSprite();
                    }
                }
                else
                {
                    bool success = ShowNativeSprite();
                    if(success == false)
                    {
                        ShowNativeIcon();
                    }
                }
            }

        }


        private bool ShowNativeSprite()
        {
            var imageTextures = AdsManager.Instance.NativeAd.GetSprites();
            if(imageTextures == null)
            {
                return false;
            }
            else if(imageTextures.Count == 0)
            {
                return false;
            }
            else
            {
                goAdLoading.SetActive(false);
                imgNativeAd.gameObject.SetActive(true);
                var sprite = imageTextures[0];
                imgNativeAd.sprite = sprite;
                List<GameObject> gameObjects = new List<GameObject>();
                gameObjects.Add(imgNativeAd.gameObject);
                int registerGameObjects = AdsManager.Instance.NativeAd.RegisterImageGameObjects(gameObjects);
                bool registerAdChoice = AdsManager.Instance.NativeAd.RegisterAdChoicesLogoGameObject(goAdChoicesLogo);
                AdsManager.Instance.NativeAd.SetUsed();
                Debug.Log($"[ShowNativeAd-ShowNativeSprite] {registerGameObjects} {registerAdChoice}");
                if(onlyShowWhenReady)
                {
                    goNativeAd.SetActive(true);
                }
                return true;
            }
        }

        private bool ShowNativeIcon()
        {
            var iconTexture = AdsManager.Instance.NativeAd.GetIconTexture();
            if(iconTexture == null)
            {
                return false;
            }
            Sprite sprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), Vector2.one * 0.5f);
            goAdLoading.SetActive(false);
            imgNativeAd.gameObject.SetActive(true);
            imgNativeAd.sprite = sprite;
            bool registerGameObjects = AdsManager.Instance.NativeAd.RegisterIconImageGameObject(imgNativeAd.gameObject);
            bool registerAdChoice = AdsManager.Instance.NativeAd.RegisterAdChoicesLogoGameObject(goAdChoicesLogo);
            AdsManager.Instance.NativeAd.SetUsed();
            Debug.Log($"[ShowNativeAd-ShowNativeIcon] {registerGameObjects} {registerAdChoice}");
            if(onlyShowWhenReady)
            {
                goNativeAd.SetActive(true);
            }
            return true;
        }
        */
    }
}
