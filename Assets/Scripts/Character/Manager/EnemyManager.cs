using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;
using Mirror;

public class EnemyManager : BaseCharacterManager
{
	private void Start()
	{
	}

	private void Update()
	{
		
	}

	[Server]
	protected override void OnCharacterAboutToBeSpawned(ECharacterType eType, ref BaseCharacter baseCharacter)
	{
		switch(eType)
		{
			case ECharacterType.enemy_zombie:
			{
				// ...
			}
			break;

			case ECharacterType.player:
			default:
			{
				Assert.IsTrue(false);
			}
			break;
		}
	}

	[Server]
	protected override void OnCharacterAboutToBeDestroyed(BaseCharacter baseCharacter)
	{
		// TODO
	}
}
