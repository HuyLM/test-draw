﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace AtoGame.Base.Helper
{
    public static class RandomHelper
    {

        public static T RandomInCollection<T>(List<T> list, List<T> blackList = null)
        {
            if (list == null || list.Count == 0)
            {
                return default(T);
            }
            T randomValue = default(T);
            do
            {
                randomValue = list[Random.Range(0, list.Count)];
            }
            while (blackList != null && blackList.Contains(randomValue));
            return randomValue;
        }

        public static T RandomInCollection<T>(T[] array, T[] blackList = null)
        {
            if (array == null || array.Length == 0)
            {
                return default(T);
            }
            T randomValue = default(T);
            do
            {
                randomValue = array[Random.Range(0, array.Length)];
            }
            while (blackList != null && blackList.Contains(randomValue));
            return randomValue;
        }

        public static T RandomInCollection<T>(T[] array, List<T> blackList)
        {
            if (array == null || array.Length == 0)
            {
                return default(T);
            }
            T randomValue = default(T);
            do
            {
                randomValue = array[Random.Range(0, array.Length)];
            }
            while (blackList != null && blackList.Contains(randomValue));
            return randomValue;
        }

        public static T[] RandomInCollection<T>(T[] array, int number, bool duplicate = false, T[] blackList = null)
        {
            if (!duplicate && array.Length < number)
            {
                Debug.LogError("Can't Random Collection because out of range");
                return null;
            }
            T[] result = new T[number];
            List<int> indexs = new List<int>();
            for (int i = 0; i < number; ++i)
            {
                int randomIndex = 0;
                do
                {
                    randomIndex = Random.Range(0, array.Length);

                } while (blackList != null && blackList.Contains(array[randomIndex]));
                if (!duplicate)
                {
                    while (indexs.Contains(randomIndex))
                    {
                        randomIndex = Random.Range(0, array.Length);
                    }
                    indexs.Add(randomIndex);
                }
                else
                {
                    indexs.Add(randomIndex);
                }
                result[i] = array[randomIndex];
            }

            return result;
        }

        public static List<T> RandomInCollection<T>(List<T> list, int number, bool duplicate = false, List<T> blackList = null)
        {
            if (!duplicate && list.Count < number)
            {
                Debug.LogError("Can't Random Collection because out of range");
                return null;
            }
            List<T> result = new List<T>();
            List<int> indexs = new List<int>();
            for (int i = 0; i < number; ++i)
            {
                int randomIndex = 0;
                do
                {
                    randomIndex = Random.Range(0, list.Count);

                } while (blackList != null && blackList.Contains(list[randomIndex]));
                if (!duplicate)
                {
                    while (indexs.Contains(randomIndex))
                    {
                        randomIndex = Random.Range(0, list.Count);
                    }
                    indexs.Add(randomIndex);
                }
                else
                {
                    indexs.Add(randomIndex);
                }
                result.Add(list[randomIndex]);
            }

            return result;
        }

        public static bool RandomWithProbability(int value)
        {
            return Random.Range(1, 101) <= value;
        }

        public static bool RandomWithProbability(float value)
        {
            return Random.Range(0, 100.0f) <= value;
        }

        public static int RandomInRange(RangeIntValue range)
        {
            return Random.Range(range.startValue, range.endValue + 1);
        }

        public static float RandomInRange(RangeFloatValue range)
        {
            return Random.Range(range.startValue, range.endValue);
        }

        public static float RandomInRange(float min, float max)
        {
            return Random.Range(min, max);
        }

        public static int RandomWithProbability(int[] probabilities)
        { // random trả về index chứa xác suất trúng
            int randomNumber = UnityEngine.Random.Range(1, 101);
            int currentProbability = 0;
            for (int i = 0; i < probabilities.Length; ++i)
            {
                if (randomNumber <= currentProbability + probabilities[i])
                {
                    return i;
                }
                currentProbability += probabilities[i];
            }
            return -1;
        }

        public static T RandomWithProbability<T>(T[] probabilities) where T : IProbability
        {
            int randomNumber = UnityEngine.Random.Range(1, 101);
            int currentProbability = 0;
            for (int i = 0; i < probabilities.Length; ++i)
            {
                if (randomNumber <= currentProbability + probabilities[i].GetProbabilities())
                {
                    return probabilities[i];
                }
                currentProbability += probabilities[i].GetProbabilities();
            }
            return default(T);
        }

        public static T RandomWithFloatProbability<T>(T[] probabilities) where T : IFloatProbability
        {
            float randomNumber = UnityEngine.Random.Range(0.0f, 100.0f);
            float currentProbability = 0;
            for (int i = 0; i < probabilities.Length; ++i)
            {
                if (randomNumber <= currentProbability + probabilities[i].GetProbabilities())
                {
                    return probabilities[i];
                }
                currentProbability += probabilities[i].GetProbabilities();
            }
            return default(T);
        }

        public static void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                int k = Random.Range(0, n);
                n--;
                T temp = list[k];
                list[k] = list[n];
                list[n] = temp;
            }
        }

        public static void Shuffle<T>(T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = Random.Range(0, n);
                n--;
                T temp = array[k];
                array[k] = array[n];
                array[n] = temp;
            }
        }
        public static void Shuffle<T, U>(T[] array, U[] array1)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = Random.Range(0, n);
                n--;
                T temp = array[k];
                array[k] = array[n];
                array[n] = temp;

                U temp1 = array1[k];
                array1[k] = array1[n];
                array1[n] = temp1;
            }
        }

    }


    public interface IProbability
    {
        int GetProbabilities();
    }

    public interface IFloatProbability
    {
        float GetProbabilities();
    }
}
