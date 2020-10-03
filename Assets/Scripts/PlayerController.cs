using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public GameObject CharacterModel;
    public float MoveSpeed = 150.0f;

    private Rigidbody rb;
    private Animator animator;

    private Vector2 InputMoveValues;

    private bool bWalking = false;

    [Client]
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
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

    // Update is called once per frame
    void Update()
    {
        Vector2 vMiddleScreen = new Vector2(Camera.main.pixelWidth * 0.5f, Camera.main.pixelHeight * 0.5f);
        Vector2 vMouse = Mouse.current.position.ReadValue();

        float angle = Mathf.Atan2(vMouse.x - vMiddleScreen.x, vMouse.y - vMiddleScreen.y) * Mathf.Rad2Deg;

        CharacterModel.transform.rotation = Quaternion.Euler(new Vector3(0.0f, angle, 0.0f));
    }
}
