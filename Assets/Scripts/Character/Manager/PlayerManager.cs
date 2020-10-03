using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : BaseCharacterManager
{
	[Server]
	protected override bool SpawnCharacterInternal(ECharacterType eType, ref BaseCharacter baseCharacter)
	{
		//
		// Retrieve transform to random spawn location
		Vector3 vPosition = new Vector3();
		Quaternion qOrn = new Quaternion();
		if (!GetSpawnParameters(ref vPosition, ref qOrn))
		{
			return false;
		}

		//
		// Instantiate prefab
		GameObject prefab = m_mapCharacterToPrefab[(int)(eType)];
		if (null == prefab)
		{
			Debug.Log("Could not spawn character '" + eType + "' from PlayerManager");
			return false;
		}
		else
		{
			GameObject newCharacter = Instantiate(prefab, vPosition, qOrn);
			baseCharacter = newCharacter.GetComponent<BaseCharacter>();
			return true;
		}
	}

	[Server]
	protected override void OnCharacterAboutToBeDestroyed(BaseCharacter baseCharacter)
	{
		// TODO
	}
}