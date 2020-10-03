/**
 *  Author : Kranck
 *  Date : 2020/10/03
 *  Description : Manages the inventory of the party
 **/

using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PartyInventory : MonoBehaviour
{
	public ScriptableObjects.Blueprint m_tempUnlockRecipe;
	private void Awake()
	{
		if (m_instance == null)
		{
			m_instance = this;
			DontDestroyOnLoad(this);
			Restart();	//< Just in case we restart
		}
		else
		{
			Destroy(this);
			m_instance.Restart();	//< in this case we restart the party manager to reset its data
		}
	}

	private List<ScriptableObjects.Blueprint> m_unlockedBlueprints = new List<ScriptableObjects.Blueprint>();
	private Dictionary<ScriptableObjects.Item, int> m_resources = new Dictionary<ScriptableObjects.Item, int>();
	private static PartyInventory m_instance;

	public List<ScriptableObjects.Item> m_availableResources = new List<ScriptableObjects.Item>();
	public static PartyInventory Instance
	{
		get { return m_instance; }
	}

	public void Restart()
	{
		//
		//
		m_unlockedBlueprints.Clear();
		
		//
		//
		m_resources.Clear();
		foreach (ScriptableObjects.Item availableResource in m_availableResources)
		{
			m_resources.Add(availableResource, 0);
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
	}

	public bool IsBlueprintUnlocked(ScriptableObjects.Blueprint blueprint)
	{
		if (m_unlockedBlueprints.Contains(blueprint))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private void OnGUI()
	{
		if (GUILayout.Button("Go !"))
		{
			SceneManager.LoadScene(0);
		}

		if (GUILayout.Button("Unlock recipe !"))
		{
			m_unlockedBlueprints.Add(m_tempUnlockRecipe);
		}
	}
}
