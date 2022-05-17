#define SafeSearch

using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class DialogPatternMatching : MonoBehaviour
{
    public DialogQuery query;
    public DialogQueryResult Result;
    // This should have been created somewhere else i guess
    public Dictionary<DialogContext, string> DialogContextDictionary = new Dictionary<DialogContext, string>();

    TextMeshPro TextMesh;
    public void Start()
    {
        foreach (DialogContext c in (DialogContext[])Enum.GetValues(typeof(DialogContext)))
            DialogContextDictionary[c] = c.ToString();

        TextMesh = GetComponent<TextMeshPro>();
    }

    public void FixedUpdate()
    {
        TextMesh.text = PatternMatch(query.context, query.worldVariables, query.variables);
    }

    public string PatternMatch(DialogContext context, DialogWorldVariables worldVariables, DialogVariables variables)
    {
        string result = string.Empty;

        result = ObtainContext(context);


        return result;
    }

    public string ObtainContext(DialogContext context)
    {
        if (!DialogContextDictionary.TryGetValue(context, out string value))
        {
            Debug.Log($"Unable to find {context.ToString()} in DialogContextDictionary.");
        }
        return value;
//#if SafeSearch
        //return DialogContextDictionary.TryGetValue(context, out string value) ? value : string.Empty;
//#endif
        //return DialogContextDictionary[context];
    }
}
