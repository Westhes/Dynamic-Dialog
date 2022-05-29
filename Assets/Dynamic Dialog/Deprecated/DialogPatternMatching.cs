#define SafeSearch

using GameName.Utility;

using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class DialogPatternMatching : MonoBehaviour
{
//    public Dictionary<DialogContext, Rules[]> DialogContextDictionary = new Dictionary<DialogContext, Rules[]>();

//    public void Start()
//    {
//        foreach (DialogContext c in (DialogContext[])Enum.GetValues(typeof(DialogContext)))
//            DialogContextDictionary[c] = new Rules[0]; // TODO: this should be 0.. make it a list maybe?
//    }

//    public Rules PatternMatch(DialogContext context, Query query)
//    {
//        Rules[] contextRules = GetContextRules(context);
//        List<Rules> validRules = new List<Rules>();

//        // Find all valid rules
//        foreach(Rules rules in contextRules)
//        {
//            if (query.RhsValidate(rules))
//            {
//                validRules.Add(rules);
//            }
//        }
        
//        // Return a random rule that matches
//        return (validRules.Count > 0) ? 
//            validRules[UnityEngine.Random.Range(0, validRules.Count -1)] : 
//            null;
//    }

//    public Rules[] GetContextRules(DialogContext context)
//    {
//        if (!DialogContextDictionary.TryGetValue(context, out Rules[] value))
//        {
//            Debug.Log($"Unable to find {context} in DialogContextDictionary.");
//        }
//        return value;
////#if SafeSearch
//        //return DialogContextDictionary.TryGetValue(context, out string value) ? value : string.Empty;
////#endif
//        //return DialogContextDictionary[context];
//    }
}
