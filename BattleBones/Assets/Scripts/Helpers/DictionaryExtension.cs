using System.Collections.Generic;

public static class DictionaryExtension
{
    /// <summary>
    /// Method responsible for increasing count of key in dictionary
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dict"></param>
    /// <param name="element"></param>
    /// <returns>True if dictionary didn't contain key before incrementation</returns>
    public static bool Increase<T>(this Dictionary<T, int> dict, T element)
    {
        if (dict.ContainsKey(element))
        {
            dict[element] += 1;
            return false;
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
    /// <returns>True if dictionary no longer contains key</returns>
    public static bool Decrease<T>(this Dictionary<T, int> dict, T element)
    {
        dict[element] -= 1;
        if (dict[element] <= 0)
        {
            dict.Remove(element);
            return true;
        }

        return false;
    }
}
