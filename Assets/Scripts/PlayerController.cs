using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public GameObject CharacterModel;
    public CharacterPlayer CharacterPlayer;

    public AudioListener PlayerAudioListener;
    public PlayerInput Inputs;

    private Rigidbody rb;
    private Animator animator;

    private Vector2 InputMoveValues;

    private bool bWalking = false;
    private bool bInteracting = false;

    private bool bResetInputs = true;

    [Client]
    void Start()
    {
        if (!hasAuthority || !isLocalPlayer)
        {
            Inputs.DeactivateInput();
            return;
        }

        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        Camera.main.gameObject.GetComponent<CameraManager>().PlayerTarget = gameObject;
    }

    [Client]
    public void OnMove(InputValue value)
    {
        if (!hasAuthority || !isLocalPlayer)
            return;

        InputMoveValues = value.Get<Vector2>();
    }

    [Client]
    public void OnAction(InputValue value)
    {
        if (bInteracting)
        {
            return;
        }

        bInteracting = true;
        animator.SetBool("Interact", true);
    }

    [Client]
    void FixedUpdate()
    {
        if (!hasAuthority || !isLocalPlayer)
            return;

        float fHorizontal = InputMoveValues.x;
        float fVertical = InputMoveValues.y;
        if (fHorizontal != 0.0f || fVertical != 0.0f)
        {
            float fSpeed = CharacterPlayer.GetCurrentSpeed() * Time.deltaTime;

            rb.velocity = new Vector3(fHorizontal * fSpeed, rb.velocity.y, fVertical * fSpeed);
            Debug.Log(InputMoveValues);
            float fAngle = 0.0f;
            if (fHorizontal >= 0.7f)
            {
                fAngle = 90.0f;
                if (fVertical >= 0.7f)
                {
                    fAngle -= 45.0f;
                }
                else if (fVertical <= -0.7f)
                {
                    fAngle += 45.0f;
                }
            }
            else if (fHorizontal <= -0.7f)
            {
                fAngle = -90.0f;
                if (fVertical >= 0.7f)
                {
                    fAngle += 45.0f;
                }
                else if (fVertical <= -0.7f)
                {
                    fAngle -= 45.0f;
                }
            }
            else if(fVertical == -1.0f)
            {
                fAngle = -180.0f;
            }

            Quaternion rota = Quaternion.identity;
            transform.rotation = Quaternion.Euler(0.0f, fAngle, 0.0f);

            bWalking = true;
            animator.SetBool("Walking", true);
        }
        else if (bWalking)
        {
            animator.SetBool("Walking", false);
            bWalking = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Interact") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            bInteracting = false;
            animator.SetBool("Interact", false);
        }
    }

    [Client]
    void Update()
    {
        if (!hasAuthority || !isLocalPlayer)
        {
            if (bResetInputs)
            {
                Inputs.enabled = false;
            }
            return;
        }
        else
        {
            if (bResetInputs)
            {
                //Inputs.SwitchCurrentActionMap(Inputs.defaultActionMap);
                Inputs.SwitchCurrentControlScheme(Inputs.defaultControlScheme);
                Inputs.ActivateInput();
                bResetInputs = false;
            }
        }
    }
}
