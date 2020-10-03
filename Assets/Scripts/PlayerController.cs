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

            //Vector3 vForward = CharacterModel.transform.forward * fVertical * fSpeed;
            //Vector3 vRight = CharacterModel.transform.right * fHorizontal * fSpeed;
            //rb.velocity = new Vector3(vForward.x + vRight.x, rb.velocity.y, vForward.z + vRight.z);

            rb.velocity = new Vector3(fHorizontal * fSpeed, rb.velocity.y, fVertical * fSpeed);
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
