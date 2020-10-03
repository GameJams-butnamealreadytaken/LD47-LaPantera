using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Mirror;

public class CharacterEnemy : BaseCharacter
{
	NavMeshAgent m_agent;

	private void Start()
	{
		m_agent = GetComponent<NavMeshAgent>();
		Assert.IsNotNull(m_agent);
	}

	private void FixedUpdate()
	{
		//m_agent.SetDestination(new Vector3(45.0f, 0.0f, 17.0f));
		//m_agent.isStopped = false;
	}

	private void Update()
	{
	}
}
