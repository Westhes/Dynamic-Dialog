namespace GameName.Utility
{
    using System.Collections.Generic;
    using GameName.DynamicDialog;
    using GameName.DynamicDialog.Blackboard.Entry;
    using GameName.DynamicDialog.Criteria;

    public static class DictionaryExtensions
    {
        /// <summary>
        /// Compares all the Keys and Values in the right handside dictionary with the left handside dictionary.
        /// </summary>
        /// <param name="lhs"> Intended to contain all the values provided by the blackboard. </param>
        /// <param name="rhs"> Requirements. </param>
        /// <returns> A score based on matching items. </returns>
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

        /// <summary>
        /// Creates a new dictionary containing provided collections, with the <paramref name="rhs"/> overwriting <paramref name="lhs"/> on collision.
        /// </summary>
        /// <param name="lhs"> Left handside. </param>
        /// <param name="rhs"> Right handside. </param>
        /// <returns> Dictionary containing the key value pairs of the combined collections. </returns>
        public static Query Combine<T1, T2>(IEnumerable<T1> lhs, IEnumerable<T2> rhs)
            where T1 : IBlackboardEntry
            where T2 : IBlackboardEntry
        {
            Query query = new Query();

            foreach (var item in lhs) query[item.Id] = item.Value;
            foreach (var item in rhs) query[item.Id] = item.Value;

            return query;
        }

        public static void Combine<T>(this Dictionary<int, float> lhs, IEnumerable<T> rhs)
            where T : IBlackboardEntry
        {
            foreach (var item in rhs) lhs[item.Id] = item.Value;
        }
    }
}