﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AtoGame.Base.UI
{
    public abstract class Collector<T> : MonoBehaviour
    {
        protected int Capacity
        {
            get; set;
        }
        protected List<T> Items
        {
            get; set;
        }
        public abstract int DisplayerCount
        {
            get;
        }

        public T GetItem(int index)
        {
            if (Items == null)
            {
                return default;
            }
            if (index < 0 || index >= Items.Count)
            {
                return default;
            }
            return Items[index];
        }

        public IEnumerable<T> GetAllItem()
        {
            if (Items != null)
            {
                foreach (T item in Items)
                {
                    yield return item;
                }
            }
        }

        public abstract void Show();

        public Collector<T> SetItems(IEnumerable<T> items)
        {
            Items = items.ToList();
            return this;
        }

        public Collector<T> SetItems(params T[] items)
        {
            Items = items.ToList();
            return this;
        }

        public Collector<T> SetItems(List<T> items)
        {
            Items = items;
            return this;
        }

        public Collector<T> SetCapacity(int capacity)
        {
            Capacity = capacity;
            return this;
        }
    }
}
