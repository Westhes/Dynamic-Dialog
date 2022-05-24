using System;
using System.Collections.Generic;
using GameName.DynamicDialog.DataTypes;

public static class DictionaryExtensions
{
    /// <summary>
    /// Compares all the Keys and Values in the right handside dictionary with the left handside dictionary.
    /// </summary>
    /// <typeparam name="T">The key type</typeparam>
    /// <typeparam name="U">The value type</typeparam>
    /// <param name="lhs">Intended to contain all the values provided by the blackboard</param>
    /// <param name="rhs">Requirements</param>
    /// <returns>A score based on matching items</returns>
    public static bool RhsValidate(this Dictionary<int, float> lhs, Dictionary<int, ICriteria> rhs)
    {
        if (rhs.Count > lhs.Count) return false;

        foreach (var rhsPair in rhs)
        {
            if (!(lhs.TryGetValue(rhsPair.Key, out float lhsValue) && rhsPair.Value.Equals(lhsValue)))
                return false;
        }

        return true;
    }

    public static Dictionary<int, float> Combine(GameName.DynamicDialog.BlackboardValue[] lhs, GameName.DynamicDialog.BlackboardValue[] rhs)
    {
        Dictionary<int, float> dict = new Dictionary<int, float>();

        foreach (var item in lhs) dict[item.Id] = item.Value;
        foreach (var item in rhs) dict[item.Id] = item.Value;

        return dict;
    }

    public static void Combine(Dictionary<int, float> lhs, GameName.DynamicDialog.BlackboardValue[] rhs)
    {
        foreach (var item in rhs) lhs[item.Id] = item.Value;
    }
}
