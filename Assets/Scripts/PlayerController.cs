using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public GameObject CharacterModel;
    public CharacterPlayer CharacterPlayer;

    public Camera PlayerCamera;
    public AudioListener PlayerAudioListener;
    public PlayerInput Inputs;

    private Rigidbody rb;
    private Animator animator;

    private Vector2 InputMoveValues;

    private bool bWalking = false;
    private bool bAttacking = false;

    [Client]
    void Start()
    {
        if (!hasAuthority || !isLocalPlayer)
        {
            //Inputs.DeactivateInput();
            return;
        }

        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    [Client]
    public void OnMove(InputValue value)
    {
        if (!hasAuthority || !isLocalPlayer)
            return;

        InputMoveValues = value.Get<Vector2>();
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

        Vector2 vMiddleScreen = new Vector2(Camera.main.pixelWidth * 0.5f, Camera.main.pixelHeight * 0.5f);
        Vector2 vMouse = Mouse.current.position.ReadValue();

        float angle = Mathf.Atan2(vMouse.x - vMiddleScreen.x, vMouse.y - vMiddleScreen.y) * Mathf.Rad2Deg;

        rb.rotation = Quaternion.Euler(new Vector3(0.0f, angle, 0.0f));
    }

    [Client]
    void Update()
    {
        if (!hasAuthority || !isLocalPlayer)
        {
            PlayerCamera.enabled = false;
            PlayerAudioListener.enabled = false;

            Inputs.enabled = false;

            return;
        }
        else
        {
            //Inputs.SwitchCurrentActionMap(Inputs.defaultActionMap);
            Inputs.SwitchCurrentControlScheme(Inputs.defaultControlScheme);
            Inputs.ActivateInput();
        }
    }
}
