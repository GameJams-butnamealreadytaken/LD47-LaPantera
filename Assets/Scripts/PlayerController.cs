﻿using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
	public GameObject CharacterModel;
	public CharacterPlayer CharacterPlayer;

	public AudioListener PlayerAudioListener;
	public PlayerInput Inputs;
	
	public List<GameObject> equipableObjects;

	private Rigidbody rb;
	private Animator animator;
	
	private Transform handBone;
	
	[SyncVar(hook = nameof(OnEquippedObjectChanged))]
	private GameObject equippedObject;

	private Vector2 InputMoveValues;

	private bool bWalking = false;
	private bool bInteracting = false;

    private bool bResetInputs = true;
    private GameObject oldEquippedObject = null;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        
        handBone = animator.GetBoneTransform(HumanBodyBones.RightHand);
        Assert.IsNotNull(handBone);
        
        if (!hasAuthority || !isLocalPlayer)
        {
            Inputs.DeactivateInput();

            return;
        }

		Camera.main.gameObject.GetComponent<CameraManager>().PlayerTarget = gameObject;
		
		CmdEquipObject(0);
	}

	[Client]
	public void OnMove(InputValue value)
	{
		if (!hasAuthority || !isLocalPlayer)
			return;

		if (!GetComponent<PlayerControllerUI>().IsInInventory())
		{
			InputMoveValues = value.Get<Vector2>();
		}
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
    
    void FixedUpdate()
    {
		//
		// Do not update if dead
		if(CharacterPlayer.GetCurrentHP() <= 0)
		{
			return;
		}
	    
        if (!hasAuthority || !isLocalPlayer)
        {
            if (oldEquippedObject != equippedObject)
            {
                OnEquippedObjectChanged(oldEquippedObject, equippedObject);
            }
            
            return;
        }

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Interact"))
		{
			float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		
			if (equippedObject != null)
			{
				equippedObject.GetComponent<Item>().SetIsInInteractFrame(normalizedTime >= 0.33f && normalizedTime <= 0.80f);
			}   
		}

		float fHorizontal = InputMoveValues.x;
		float fVertical = InputMoveValues.y;
		if (fHorizontal != 0.0f || fVertical != 0.0f)
		{
			float fSpeed = CharacterPlayer.GetCurrentSpeed() * Time.deltaTime;

			rb.velocity = new Vector3(fHorizontal * fSpeed, rb.velocity.y, fVertical * fSpeed);

			float fAngle = 0.0f;
			if (fHorizontal >= 0.1f)
			{
				fAngle = 90.0f;
				if (fVertical > 0.0f)
				{
					fAngle -= 45.0f;
				}
				else if (fVertical < -0.0f)
				{
					fAngle += 45.0f;
				}
			}
			else if (fHorizontal < -0.0f)
			{
				fAngle = -90.0f;
				if (fVertical > 0.0f)
				{
					fAngle += 45.0f;
				}
				else if (fVertical < -0.0f)
				{
					fAngle -= 45.0f;
				}
			}
			else if(fVertical < 0.0f)
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
			rb.velocity = Vector3.zero;
			animator.SetBool("Walking", false);
			bWalking = false;
		}

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Interact") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
		{
			bInteracting = false;
			animator.SetBool("Interact", false);
		}
	}
	
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

	[Command]
	private void CmdEquipObject(int objectIndex)
	{
		if (equippedObject != null)
		{
			Destroy(equippedObject);
			equippedObject = null;
		}

		if (objectIndex < 0)
		{
			return;
		}
		
		GameObject spawnedObject = Instantiate(equipableObjects[objectIndex], handBone.position, handBone.rotation);
		spawnedObject.transform.SetParent(handBone);
		
		NetworkServer.Spawn(spawnedObject);

		equippedObject = spawnedObject;
	}

    [Client]
    private void OnEquippedObjectChanged(GameObject oldEquippedObject, GameObject newEquippedObject)
    {
        if (!GetComponent<NetworkIdentity>().isServer && equippedObject != null)
        {
	        equippedObject.transform.SetParent(null);
	        
            equippedObject.transform.position = handBone.position;
            equippedObject.transform.rotation = handBone.rotation;
            equippedObject.transform.localScale = Vector3.one;
            equippedObject.transform.SetParent(handBone);   
        }
    }
}
