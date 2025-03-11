using System;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationSwitcher : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;
    Vector2 privateMoveVector;

    public GameObject player;
    public Animator animator;
    public AnimatorController idle;
    public AnimatorController walk;
    public SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        //animator.runtimeAnimatorController = walk;
    }

    // Update is called once per frame
    void Update()
    {
        // flip character to face direction of moveVector
        privateMoveVector = player.GetComponent<PlayerController>().moveVector;
        if (privateMoveVector.x < 0)
        {
            animator.SetBool("isLeft", true);
            sr.flipX = true;
        } 
        else if (privateMoveVector.x > 0)
        {
            animator.SetBool("isLeft", false);
            sr.flipX = false;
        }

        if (moveAction.IsPressed())
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
