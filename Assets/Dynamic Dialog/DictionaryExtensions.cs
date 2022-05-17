using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DictionaryExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The key type</typeparam>
    /// <param name="lhs">Intended to contain all the values provided by the blackboard</param>
    /// <param name="rhs">Requirements</param>
    /// <returns>A score based on matching items</returns>
    public static bool Validate<T>(Dictionary<T, object> lhs, Dictionary<T, object> rhs)
    {
        if (rhs.Count > lhs.Count) return false;

        foreach (var rhsPair in rhs)
        {
            if (!(lhs.TryGetValue(rhsPair.Key, out object lhsValue) && rhsPair.Value.Equals(lhsValue)))
                return false;
        }
        return true;
    }
}
