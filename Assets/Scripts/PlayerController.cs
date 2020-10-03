using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public float MoveSpeed = 150.0f;

    private Rigidbody rb;
    private Animator animator;

    private Vector2 InputMoveValues;

    private bool bWalking = false;

    [Client]
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    [Client]
    public void OnMove(InputValue value)
    {
        InputMoveValues = value.Get<Vector2>();
    }

    [Client]
    void FixedUpdate()
    {
        if (!hasAuthority)
            return;

        float fHorizontal = InputMoveValues.x;
        float fVertical = InputMoveValues.y;
        if (fHorizontal != 0.0f || fVertical != 0.0f)
        {
            float fSpeed = MoveSpeed * Time.deltaTime;
            rb.velocity = new Vector3(fHorizontal * fSpeed, rb.velocity.y, fVertical * fSpeed);
            bWalking = true;
            animator.SetBool("Walking", true);
        }
        else if (bWalking)
        {
            animator.SetBool("Walking", false);
            bWalking = false;
        }
    }
}

