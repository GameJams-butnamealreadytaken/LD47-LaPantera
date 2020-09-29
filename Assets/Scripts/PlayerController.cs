using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float MoveSpeed = 150.0f;
    public float JumpForce = 20.0f;
    
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Animator animator;

    private bool bWalking = false;
    private bool bJumping = false;

    [Client]
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    [Client]
    void FixedUpdate()
    {
        if (!hasAuthority)
            return;

        if (!bJumping && Input.GetAxis("Jump") != 0.0f)
        {
            rb.AddForce(0.0f, JumpForce, 0.0f);
            bJumping = true;
            animator.SetBool("Jumping", true);
        }

        float fHorizontal = Input.GetAxis("Horizontal");
        float fVertical = Input.GetAxis("Vertical");
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

        if (bJumping)
        {
            if (rb.velocity.y < 0.0f && Physics.CheckSphere(transform.position, 0.1f, groundLayer.value))
            {
                bJumping = false;
                animator.SetBool("Jumping", false);
            }
        }
    }
}
