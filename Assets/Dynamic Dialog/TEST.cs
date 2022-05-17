using GameName.DynamicDialog.DataTypes;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    Query Query = new Query();
    public Rules rules = new Rules();

    //public IntLessOrEquals lessThanEquals;
    public CriterionValue Criterion;

    public void Start()
    {
        Query.Add(DialogWorldVariables.Everything, "Test");
        Query.Add(DialogWorldVariables.IsAlive, true);
        Query.Add(DialogWorldVariables.ScoreCount, 5);
        Query.Add(DialogWorldVariables.DeathCount, 1);

        rules.Add(DialogWorldVariables.IsAlive, true);
        rules.Add(DialogWorldVariables.ScoreCount, Criterion);

        bool rulesAreMet = DictionaryExtensions.RhsValidate(Query, rules);
        Debug.Log($"Rules are met: {rulesAreMet} {(rulesAreMet ? rules.Response.Text : "..")}");
    }
}
