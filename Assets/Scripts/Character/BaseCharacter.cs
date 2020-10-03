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
	public int m_iSpeed = 10;
	public int m_iMaxHP = 5;
	public int m_iAttackStrength = 1;
	public int m_iAttackRange = 3;

	[Header("Loot")]
	public int m_iLootCountMin = 1;
	public int m_iLootCountMax = 1;
	public ScriptableObjects.Item m_iLootItem;


	[SyncVar]
	private int m_iCurrentHP;
	[SyncVar]
	private int m_iCurrentSpeed;
	[SyncVar]
	private int m_iCurrentAttackStrength;
	[SyncVar]
	private int m_iCurrentAttackRange;
	[SyncVar]
	private string m_strName;

	// Start is called before the first frame update
	protected void Start()
    {
		ResetCharacteristicsToInitialValues();
	}

	[Command]
	public void ResetCharacteristicsToInitialValues()
	{
		//
		// Current characteristic = initial characteristic
		m_iCurrentHP = m_iMaxHP;
		m_iCurrentSpeed = m_iSpeed;
		m_iCurrentAttackStrength = m_iAttackStrength;
		m_iCurrentAttackRange = m_iAttackRange;
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
}
