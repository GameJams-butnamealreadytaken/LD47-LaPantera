﻿/**
 *  Author : Kranck
 *  Date : 2020/10/03
 *  Description : Manages the inventory of the party
 **/

using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PartyInventory : MonoBehaviour
{
	public ScriptableObjects.Blueprint m_tempUnlockRecipe; 	//TODO: Remove
	
	/// <summary>
	/// Awake is used to reset the singleton
	/// </summary>
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

	private List<ScriptableObjects.Blueprint> m_unlockedBlueprints = new List<ScriptableObjects.Blueprint>();	//< The blueprints that has been unlocked 
	private Dictionary<string, int> m_resources = new Dictionary<string, int>();	// The amount of resources of each type (by name) that the party has

	/// <summary>
	/// The resources that the party has in the form of dicionary : Name of resource/item -> amount
	/// </summary>
	public Dictionary<string, int> Resources
	{
		get { return m_resources;  }
	}
	
	private static PartyInventory m_instance;	// Singleton instance
	/// <summary>
	/// Instance of the party inventory
	/// </summary>
	public static PartyInventory Instance
	{
		get { return m_instance; }
	}

	[SerializeField]
	[Tooltip("The array of resources (items) that are available in this game")]
	private List<ScriptableObjects.Item> m_availableResources = new List<ScriptableObjects.Item>();

	// TODO: Remove ->
	public ScriptableObjects.Item m_tmpWood;
	public ScriptableObjects.Item m_tmpRock;
	

	/// <summary>
	/// Restart the party inventory (reset)
	/// </summary>
	public void Restart()
	{
		//
		// Clear the unlocked blueprints
		m_unlockedBlueprints.Clear();
		
		//
		// Clear the resources owned 
		m_resources.Clear();
		
		//
		// Initialize the resources array by creating a case for each avaialable resource
		foreach (ScriptableObjects.Item availableResource in m_availableResources)
		{
			m_resources.Add(availableResource.name, 0);
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
	
	/// <summary>
	/// Credit (add) the given amount of the given resources to the party invenntory
	/// </summary>
	/// <param name="_creditedItem">The item to credit</param>
	/// <param name="_amount">The amount of items to credit</param>
	public void CreditResource(ScriptableObjects.Item _creditedItem, int _amount = 1)
	{
		m_resources[_creditedItem.name] += _amount;
	}

	/// <summary>
	/// Uncredit (remove) the given amount of the given resources from the party invenntory
	/// </summary>
	/// <param name="_creditedItem">The item to uncredit</param>
	/// <param name="_amount">The amount of items to uncredit</param>
	public void UnCreditResource(ScriptableObjects.Item _creditedItem, int _amount = 1)
	{
		m_resources[_creditedItem.name] -= _amount;
		if (m_resources[_creditedItem.name] < 0)
		{
			m_resources[_creditedItem.name] = 0;
		}
	}

	/// <summary>
	/// Return true if the given blueprint is unlocked
	/// WARNING : A blueprint that starts unlocked will never be unlocked  according to this method. It has to be used only
	/// for blueprints that are locked at the beginning of the game
	/// </summary>
	/// <param name="blueprint">The blueprint we want to check if it is unlocked (must be a blueprint that is locked at the beginning of the game)</param>
	/// <returns>True if this blueprint is unlocked or not</returns>
	public bool IsBlueprintUnlocked(ScriptableObjects.Blueprint blueprint)
	{
		//
		// We check in the unlocked blueprints if our blueprint is unlocked
		if (m_unlockedBlueprints.Contains(blueprint))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Returns true if the party has more or at least the number of resources given
	/// </summary>
	/// <param name="_item">The type of resource</param>
	/// <param name="_count">The count of resources we want to check for</param>
	/// <returns></returns>
	public bool HasEnoughResources(ScriptableObjects.Item _item, int _count)
	{
		if (m_resources[_item.name] >= _count)
		{
			return true;
		}

		return false;
	}

	public bool CanCraft(ScriptableObjects.Blueprint _blueprint)
	{
		//
		// Iterate over all the recipe elements
		foreach (var recipeItem in _blueprint.RecipeItems)
		{
			//
			// If we don't have enough resources, then we can't craft this blueprint
			if (!HasEnoughResources(recipeItem.m_item, recipeItem.m_quantity))
			{
				return false;
			}
		}

		//
		// We have enough fo all the neededd resources, we can craft the blueprint
		return true;
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

		if (GUILayout.Button("Add Wood *2 "))
		{
			CreditResource(m_tmpWood, 2);
		}
		
		if (GUILayout.Button("Add Rock *3 "))
		{
			CreditResource(m_tmpRock, 3);
		}
	}
}
