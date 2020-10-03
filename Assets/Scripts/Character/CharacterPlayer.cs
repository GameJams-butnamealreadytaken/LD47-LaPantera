using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CharacterPlayer : BaseCharacter
{
	protected new void Start()
	{
		base.Start();

		Assert.IsNotNull(m_characterManager);
		Assert.IsTrue(m_characterManager is PlayerManager);
	}
}
