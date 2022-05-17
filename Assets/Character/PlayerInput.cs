using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : Controller
{
    private Vector2 inputDirection;
    private bool pressedJump;

    public override void Process(ref InputManager.InputState control)
    {
        control.InputDirection = inputDirection;
        control.Jump = pressedJump;
        base.Process(ref control);
    }

    void Update()
    {
        // Continuously update the input, so when the data is requested it can be provided.
        inputDirection = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );
        pressedJump = Input.GetAxis("Jump") != 0;
    }
}
