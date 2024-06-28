using AtoGame.Base;
using AtoGame.OtherModules.Inventory;
using AtoGame.OtherModules.LocalSaveLoad;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickyBrain
{
    public class InventorySaveData : IItemInventorySaver, LocalSaveLoadable
    {
        private const string KEY = "ItemInventorySaveData";

        private ItemData[] items;
        private int[] infiniteIds;
        private Action onIntialized;
        private bool isInitialized;



        public InventorySaveData()
        {
            isInitialized = false;
        }

        #region IItemInventorySaver
        public void AddOnInit(Action onCompleted)
        {
            if (isInitialized == true)
            {
                onCompleted?.Invoke();
            }
            else
            {
                onIntialized = onCompleted;
            }
        }

        public void Load(Action<bool, ItemData[], int[]> onLoaded)
        {
            onLoaded?.Invoke(true, items, infiniteIds);
        }

        public void PushSave(ItemData[] items, List<int> infiniteIds, Action<bool> onResult)
        {
            this.items = items;
            this.infiniteIds = infiniteIds == null ? null : infiniteIds.ToArray();
            SaveData();
            onResult?.Invoke(true);
        }
        #endregion

        #region LocalSaveLoadable
        public void CreateData()
        {
            infiniteIds = null;
        }
        public void InitData()
        {
            isInitialized = true;
            if (onIntialized != null)
            {
                onIntialized.Invoke();
            }
        }

        public void LoadData()
        {
            SaveDataModel saveData = null;
            string json = PlayerPrefExtension.GetString(KEY, string.Empty);
            if (string.IsNullOrEmpty(json) == false)
            {
                saveData = JsonConvert.DeserializeObject<SaveDataModel>(json);
            }
            if (saveData == null)
            {

            }
            else
            {
                if (saveData.Ids != null && saveData.Amounts != null && saveData.Ids.Length > 0 && saveData.Amounts.Length > 0)
                {
                    if (saveData.Ids.Length == saveData.Amounts.Length)
                    {
                        items = new ItemData[saveData.Ids.Length];
                        for (int i = 0; i < items.Length; ++i)
                        {
                            items[i] = new ItemData(saveData.Ids[i], saveData.Amounts[i]);
                        }
                    }
                }

                if (saveData.InfiniteItemIds != null && saveData.InfiniteItemIds.Length > 0)
                {
                    infiniteIds = saveData.InfiniteItemIds;
                }
                else
                {
                    infiniteIds = null;
                }
            }
        }

        public void SaveData()
        {
            SaveDataModel saveData = new SaveDataModel();
            if (items != null && items.Length > 0)
            {
                saveData.Ids = new int[items.Length];
                saveData.Amounts = new long[items.Length];
                for (int i = 0; i < items.Length; ++i)
                {
                    saveData.Ids[i] = items[i].Id;
                    saveData.Amounts[i] = items[i].Amount;
                }
            }
            if (infiniteIds != null && infiniteIds.Length > 0)
            {
                saveData.InfiniteItemIds = infiniteIds;
            }

            string json = JsonConvert.SerializeObject(saveData);
            PlayerPrefExtension.SetString(KEY, json);
        }

        public void EaseData()
        {
            CreateData();
        }

        private class SaveDataModel
        {
            [JsonProperty] private int[] ids;
            [JsonProperty] private long[] ams;
            [JsonProperty] private int[] iii;

            [JsonIgnore]
            public int[] Ids
            {
                get => ids;
                set => ids = value;
            }
            [JsonIgnore]
            public long[] Amounts
            {
                get => ams;
                set => ams = value;
            }
            [JsonIgnore]
            public int[] InfiniteItemIds
            {
                get => iii;
                set => iii = value;
            }
        }
        #endregion
    }
}
