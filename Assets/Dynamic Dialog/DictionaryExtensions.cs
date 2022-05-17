using System.Collections.Generic;

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
    public static bool RhsValidate<T, U>(this Dictionary<T, U> lhs, Dictionary<T, U> rhs)
    {
        if (rhs.Count > lhs.Count) return false;

        foreach (var rhsPair in rhs)
        {
            if (!(lhs.TryGetValue(rhsPair.Key, out U lhsValue) && rhsPair.Value.Equals(lhsValue)))
                return false;
        }
        return true;
    }
}
