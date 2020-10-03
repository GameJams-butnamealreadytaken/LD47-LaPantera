using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Mirror;

public class CharacterEnemy : BaseCharacter
{
	[Header("Enemy properties")]
	public float m_fAggroRadius = 10.0f;
	public bool m_bShowAggroRadius = false;

	private NavMeshAgent m_agent;

	private new void Start()
	{
		base.Start();

		m_agent = GetComponent<NavMeshAgent>();
		Assert.IsNotNull(m_agent);
	}

	void OnDrawGizmos()
	{
		if (m_bShowAggroRadius)
		{
			UnityEditor.Handles.color = Color.red;
			UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, m_fAggroRadius);
		}
	}

	private void FixedUpdate()
	{
		m_agent.SetDestination(new Vector3(45.0f, 0.0f, -17.0f));
		m_agent.isStopped = false;
	}

	private void Update()
	{
	}

}
