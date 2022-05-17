using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class FunctionSelectorTest : MonoBehaviour
{
    private enum FunctionOption
    {
        FunctionA,
        FunctionB,
        FunctionC
    };

    [SerializeField]
    private FunctionOption _selectedFunction;
    private Dictionary<FunctionOption, System.Action> _functionLookup;

    public UnityEvent test;
    

    private void Awake()
    {
        _functionLookup = new Dictionary<FunctionOption, System.Action>()
        {
            { FunctionOption.FunctionA, FunctionA },
            { FunctionOption.FunctionB, FunctionB },
            { FunctionOption.FunctionC, FunctionC }
        };
    }


    public void ActivateSelectedFunction()
    {
        _functionLookup[_selectedFunction].Invoke();
    }


    private void FunctionA() { }

    private void FunctionB() { }

    private void FunctionC() { }
}