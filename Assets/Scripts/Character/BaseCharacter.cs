using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum ECharacterType
{
	player,
	enemy_zombie,
};

public class BaseCharacter : NetworkBehaviour
{
	[Header("Manager")]
	public BaseCharacterManager m_characterManager;

	[Header("Characteristics")]
	public int m_iMaxHP = 5;
	public int m_fSpeed = 10;
	public int m_fAttackStrength = 1;
	public int m_fAttackRange = 3;

	[Header("Loot")]
	public int m_iLootCountMin = 1;
	public int m_iLootCountMax = 1;
	public ScriptableObjects.Item m_iLootItem;


	[SyncVar]
	private int m_iCurrentHP;
	[SyncVar]
	private float m_fCurrentSpeed;
	[SyncVar]
	private float m_fCurrentAttackStrength;
	[SyncVar]
	private float m_fCurrentAttackRange;
	[SyncVar]
	private string m_strName;

	// Start is called before the first frame update
	protected void Start()
    {
		if (hasAuthority)
			ResetCharacteristicsToInitialValues();
	}

	[Command]
	public void ResetCharacteristicsToInitialValues()
	{
		//
		// Current characteristic = initial characteristic
		m_iCurrentHP = m_iMaxHP;
		m_fCurrentSpeed = m_fSpeed;
		m_fCurrentAttackStrength = m_fAttackStrength;
		m_fCurrentAttackRange = m_fAttackRange;
	}

	[Server]
	private void OnCharacterDeath() 
	{
		// TODO
		// - drop item

		//
		// Call associated manager for destruction
		m_characterManager.DestroyCharacter(this);
	}

	[Command]
	private void TakeDamage(int iDamage)
	{
		m_iCurrentHP = Mathf.Max(m_iCurrentHP - iDamage, 0);
		if(m_iCurrentHP == 0)
		{
			OnCharacterDeath();
		}
	}
	
	public int GetCurrentHP()
	{
		return m_iCurrentHP;
	}
	
	public float GetCurrentSpeed()
	{
		return m_fCurrentSpeed;
	}
	
	public float GetCurrentAttackStrength()
	{
		return m_fCurrentAttackStrength;
	}
	
	public float GetCurrentAttackRange()
	{
		return m_fCurrentAttackRange;
	}
}
