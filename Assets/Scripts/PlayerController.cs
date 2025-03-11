using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    InputAction moveAction;
    InputAction jumpAction;
    InputAction shoot;
    public Vector2 moveVector;
    public Animator animator;
    public ParticleSystem throwingArm;
    public GameObject snowballPrefab;   // Prefab to instantiate when firing
    public float snowballSpeed = 10f;     // Speed at which the snowball is fired

    Rigidbody2D rb;
    float accelSpeed = 25.0f;
    float maxSpeed = 8.5f;
    float jumpForce = 600;

    bool pressingButton;
    bool isJumping;
    bool isLeft;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        shoot = InputSystem.actions.FindAction("Attack");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        
        if (shoot.WasPerformedThisFrame())
        {
            FireSnowball();
        }

        if (moveVector.x < 0 && isLeft == false)
        {
            isLeft = true;
            throwingArm.transform.Rotate(new Vector2(0, 180));
        }
        if (moveVector.x > 0 && isLeft == true)
        {
            isLeft = false;
            throwingArm.transform.Rotate(new Vector2(0, 180));
        }
    }

    void FixedUpdate()
    {
        if (isJumping)
        {
            rb.AddForceY(jumpForce);
            isJumping = false;
        }
        if (pressingButton)
        {
            rb.AddForce(moveVector);
        }
        else if (Math.Abs(rb.linearVelocityX) == 0)
        {
            return;
        }
        else if (Math.Abs(rb.linearVelocityX) != 0)
        {
            float targetVelocity = 0f; // We want velocity to approach 0
            float smoothTime = 0.1f;   // Time to smooth
            float velocityRef = 0f;    // Reference velocity required by SmoothDamp

            // Apply smooth damping to the x velocity
            float newVelocityX = Mathf.SmoothDamp(rb.linearVelocity.x, targetVelocity, ref velocityRef, smoothTime);

            // Apply the new velocity while keeping the y velocity unchanged
            rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);
        }
    }

    void Move()
    {
        // if moving but not pressing anything, apply backwards speed to slow down
        // if vector is zero before resetting it, no button pressed
        moveVector = moveAction.ReadValue<Vector2>();
        if (moveVector == Vector2.zero)
        {
            pressingButton = false;
            return;
        }

        // reset vector so as to not add more speed
        moveVector *= accelSpeed;
        if (Math.Abs(rb.linearVelocityX) > maxSpeed)
        {
            moveVector = Vector2.zero;
        }
        pressingButton = true;

    }

    void Jump()
    {
        if (jumpAction.WasPerformedThisFrame())
        {
            Debug.Log("Jumped");
            isJumping = true;
        }
    }

    void FireSnowball()
    {
        // Determine horizontal direction based on player's facing
        Vector2 direction = isLeft ? Vector2.left : Vector2.right;

        // Define an offset value for the snowball spawn position
        float offsetX = 1.0f; // Adjust this value based on your player's size and requirements
        Vector3 offset = isLeft ? new Vector3(-offsetX, 0, 0) : new Vector3(offsetX, 0, 0);

        // Instantiate the snowball at the throwing arm's position plus the offset
        GameObject snowball = Instantiate(snowballPrefab, throwingArm.transform.position + offset, Quaternion.identity);

        // Set the snowball's velocity if it has a Rigidbody2D
        Rigidbody2D snowballRb = snowball.GetComponent<Rigidbody2D>();
        if (snowballRb != null)
        {
            snowballRb.linearVelocity = direction * snowballSpeed;
        }
    }
}
