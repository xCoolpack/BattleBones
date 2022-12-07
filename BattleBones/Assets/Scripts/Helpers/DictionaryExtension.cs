using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DictionaryExtension
{
    /// <summary>
    /// Method responsible for increasing count of key in dictionary
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dict"></param>
    /// <param name="element"></param>
    /// <returns>True if dictionary didn't contain key or has value 0 before incrementation</returns>
    public static bool Increase<T>(this Dictionary<T, int> dict, T element)
    {
        if (dict.ContainsKey(element))
        {
            dict[element] += 1;
            return dict[element] <= 1;
        }

        dict.Add(element, 1);
        return true;
    }

    /// <summary>
    /// Method responsible for decreasing count of key in dictionary
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dict"></param>
    /// <param name="element"></param>
    /// <returns>True if key dictionary has value 0</returns>
    public static bool Decrease<T>(this Dictionary<T, int> dict, T element)
    {
        dict[element] = System.Math.Max(dict[element] - 1, 0);
        return dict[element] <= 0;
    }
}
