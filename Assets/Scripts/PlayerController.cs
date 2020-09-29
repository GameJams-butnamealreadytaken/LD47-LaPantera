using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float MoveSpeed = 50.0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    [Client]
    void FixedUpdate()
    {
        if (!hasAuthority)
            return;

        float fHorizontal = Input.GetAxis("Horizontal");
        float fVertical = Input.GetAxis("Vertical");

        float fSpeed = MoveSpeed * Time.deltaTime;
        rb.velocity = new Vector3(fHorizontal * fSpeed, rb.velocity.y, fVertical * fSpeed);
    }
}
