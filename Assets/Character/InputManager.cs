using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InputManager
{
    public struct InputState
    {
        public Vector2 InputDirection { get; set; }

        public bool Jump { get; set; }
    }

    public InputState Input { get; private set; } = new InputState();
    private Controller initialController = null;
    [SerializeReference] private Controller[] controllers = null;

    public void Initialize()
    {
        foreach (var c in controllers) AddController(c);
        // Free resources
        controllers = null;
    }

    //public InputManager(params Controller[] values)
    //{
    //    foreach (var c in values) AddController(c);
    //}

    /// <summary> Updates Input </summary>
    public void UpdateInput()
    {
        // Obtain control input
        var input = new InputState();
        initialController?.Process(ref input);
        Input = input;
    }

    public void AddController(Controller controller)
    {
        if (initialController == null)
        {
            initialController = controller;
            return;
        }

        Controller c = initialController;
        while (c.NextLink != null)
        {
            c = (Controller)c.NextLink;
        }
        c.NextLink = controller;
    }
}
