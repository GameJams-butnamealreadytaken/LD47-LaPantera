using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class BaseCharacterManager : NetworkBehaviour
{
	[Header("Spawn")]
	public List<Vector2> m_aSpawnLocations;

	[Header("Character prefabs")]
	[Tooltip("0 = character\n1 = enemy_zombie")]
	public GameObject[] m_mapCharacterToPrefab = new GameObject[System.Enum.GetNames(typeof(ECharacterType)).Length];

	private List<BaseCharacter> m_aCharacters = new List<BaseCharacter>();

	[Server]
	protected bool GetSpawnParameters(ref Vector3 vPosition, ref Quaternion qOrn)
	{
		if(m_aSpawnLocations.Count == 0)
		{
			return false;
		}

		System.Random r = new System.Random();
		Vector2 vXYLocation = m_aSpawnLocations[r.Next(m_aSpawnLocations.Count)];
		vPosition.Set(vXYLocation.x, vXYLocation.y, 0.0f);
		//qOrn = Quaternion.Euler(0.0f, 0.0f, (float)(r.NextDouble() * 360.0f));
		qOrn = Quaternion.identity;

		return true;
	}

	[Server]
	public bool SpawnCharacter(ECharacterType eType, ref BaseCharacter baseCharacter)
	{
		if(SpawnCharacterInternal(eType, ref baseCharacter))
		{
			//
			// Spawn character on server
			NetworkServer.Spawn(baseCharacter.gameObject);

			//
			// Add character to list
			m_aCharacters.Add(baseCharacter);

			return true;
		}

		return false;
	}

	protected abstract bool SpawnCharacterInternal(ECharacterType eType, ref BaseCharacter baseCharacter);

	protected abstract void OnCharacterAboutToBeDestroyed(BaseCharacter baseCharacter);

	[Server]
	public bool DestroyCharacter(BaseCharacter character)
	{
		//
		// Remove object from list
		if(m_aCharacters.Remove(character))
		{
			//
			// Notify Character destruction
			OnCharacterAboutToBeDestroyed(character);

			//
			// Destroy gameobject
			NetworkServer.Destroy(character.gameObject);

			return true;
		}

		return false;
	}
}
