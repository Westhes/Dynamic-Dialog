using GameName.DynamicDialog.DataTypes;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    Query<Enum, object> Query = new Query<Enum, object>();
    Query<Enum, object> Rules = new Query<Enum, object>();

    public IntLessOrEquals lessThanEquals;

    public void Start()
    {
        Query.Add(DialogWorldVariables.Everything, "Test");
        Query.Add(DialogWorldVariables.IsAlive, true);
        Query.Add(DialogWorldVariables.ScoreCount, 5);
        Query.Add(DialogWorldVariables.DeathCount, 1);

        //foreach (var kvp in Query)
        //{
        //    Debug.Log($"K: {kvp.Key} V: {kvp.Value}");
        //}
        Rules.Add(DialogWorldVariables.IsAlive, true);
        Rules.Add(DialogWorldVariables.ScoreCount, lessThanEquals);

        bool rulesAreMet = DictionaryExtensions.Validate(Query, Rules);
        Debug.Log($"Rules are met: {rulesAreMet}");
    }


}
