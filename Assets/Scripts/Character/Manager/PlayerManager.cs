using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : BaseCharacterManager
{

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