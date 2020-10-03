using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class EnemyManager : BaseCharacterManager
{
	private void Start()
	{
		//List<BaseCharacter> d = new List<BaseCharacter>();
		//if(SpawnCharacters(ECharacterType.enemy_zombie, 520, ref d))
		//{
		//	Debug.Log("Spawn success");
		//}
	}

	[Server]
	protected override void OnCharacterAboutToBeSpawned(ECharacterType eType, ref BaseCharacter baseCharacter)
	{
		// TODO
	}

	[Server]
	protected override void OnCharacterAboutToBeDestroyed(BaseCharacter baseCharacter)
	{
		// TODO
	}
}
