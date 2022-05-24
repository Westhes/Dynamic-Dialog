using GameName.DynamicDialog.DataTypes;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public FactEntry entry;

    Query Query = new Query();
    public Rules rules = new Rules();

    public CriterionStatic isAliveCriterion;
    public CriterionValue scoreCriterion;

    public void Start()
    {
        // Create query / blackboard
        // This can change every frame/check and can contain many elements that might not even be read
        //Query.Add((int)DialogWorldVariables.Everything, "Test");
        Query.Add((int)DialogWorldVariables.IsAlive, true.GetHashCode());  // This will be checked.
        Query.Add((int)DialogWorldVariables.ScoreCount, 20); // This will be checked.
        Query.Add((int)DialogWorldVariables.DeathCount, 1);

        // Create a criteria object which can be set from outside codes.
        //isAliveCriterion = true;
        isAliveCriterion = new CriterionStatic(true);
        scoreCriterion = new CriterionValue(10f, ConditionOperator.GreaterOrEquals);

        // Create rules that must be met
        // Rules will be set once, probably at launch.
        rules.Add((int)DialogWorldVariables.IsAlive, isAliveCriterion);
        rules.Add((int)DialogWorldVariables.ScoreCount, scoreCriterion);
        rules.Response = new Response("Hello world");

        // Validate
        bool areRulesMet = DictionaryExtensions.RhsValidate(Query, rules);
        Debug.Log($"Rules are met: {areRulesMet} {(areRulesMet ? rules.Response.Text : "..")}"); 
        // Outputs: Rules are met: True Hello world
    }

    private void Update()
    {
        entry.Value++;
    }
}
