using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;

    private Rigidbody2D rb;
    private new Collider2D collider;
    public float force = 1000;
    public float jumpHeight = 1;
    private float jumpStartTime = 0f;
    public float walkSpeed = 1;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        inputManager.Initialize();
    }

    private void Update()
    {
        inputManager.UpdateInput();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(inputManager.Input.InputDirection.x * walkSpeed, rb.velocity.y);

        if (inputManager.Input.Jump && Time.time > jumpStartTime)
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, 1.1f, 1 << LayerMask.NameToLayer("Floor"));
            if (hit)
            {
                // Sub-par way of adding jump force, since downwards momentum might still be at play.
                rb.AddForce(new Vector2(0, jumpHeight * force));
                jumpStartTime = Time.time + 0.1f;
            }
        }
    }
}
