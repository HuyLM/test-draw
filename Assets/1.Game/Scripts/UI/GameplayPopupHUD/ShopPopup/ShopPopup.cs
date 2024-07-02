using AtoGame.OtherModules.HUD;
using AtoGame.OtherModules.Inventory;
using AtoGame.OtherModules.LocalSaveLoad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class ShopPopup : BasePopup
    {
        [SerializeField] private ShopItemCollector shopItemCollector;


        protected override void ActiveFrame()
        {
            base.ActiveFrame();

            var config = DataConfigs.Instance.ShopConfigData;
            shopItemCollector.SetOnSelected(OnShopItemSelected).SetCapacity(config.ShopItemConfigDatas.Count).SetItems(config.ShopItemConfigDatas).Show();
        }

        private void OnShopItemSelected(ShopItemDisplayer displayer)
        {
            var config = DataConfigs.Instance.ShopConfigData;
            var saveData = LocalSaveLoadManager.Get<ShopSaveData>();

            bool isUnlocked = displayer.Model.IsUnlocked;
            bool isUsing = displayer.Model.Id == saveData.UsingItemID;

            if(isUsing)
            {
                return;
            }
            if(isUnlocked)
            {
                int preUsingID = saveData.UsingItemID;
                saveData.UsingItemID = displayer.Model.Id;
                var preUsingDisplayer = shopItemCollector.GetShopItemDipslayer(preUsingID);
                if(preUsingDisplayer != null)
                {
                    preUsingDisplayer.Show();
                }
                displayer.Show();
                saveData.SaveData();
                GameSoundManager.Instance.PlaySelectPencil();
                return;
            }
            if(displayer.Model.UnlockLevel == -1)
            {
                if(AdsManager.Instance.IsRewardVideoReady())
                {
                    GameTracking.LogShowAds(true, "feature_unlock_pencil");
                    AdsManager.Instance.ShowRewardVideo("feature_unlock_pencil", () => {
                        displayer.Model.Claim(1, "shop_popup");
                        displayer.Show();
                        ItemInventoryController.Instance.Save();
                    }, () => {
                        GameTracking.LogShowAds(false, "feature_unlock_pencil");
                        string message = "key_show_ad_failed_message";
                        string title = "key_show_ad_failed_title";
                        NoticePopup noticePopup = PopupHUD.Instance.Show<NoticePopup>();
                        noticePopup.SetMessage(message);
                        noticePopup.SetTitle(title);
                    });
                }
                else
                {
                    GameTracking.LogShowAds(false, "feature_unlock_pencil");
                    string message = "key_ad_not_availiable_message";
                    string title = "key_ad_not_availiable_title";
                    NoticePopup noticePopup = PopupHUD.Instance.Show<NoticePopup>();
                    noticePopup.SetMessage(message);
                    noticePopup.SetTitle(title);
                }
                return;
            }
        }
    }
}
