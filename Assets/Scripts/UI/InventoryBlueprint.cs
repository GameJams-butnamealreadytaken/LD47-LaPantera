/**
 *  Author : Kranck
 *  Date : 2020/10/03
 *  Description : A blueprint on the inventory. It can be selected to craft an item
 **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class InventoryBlueprint : MonoBehaviour
{
	
	#region Variables
	
		[Tooltip("The data of the blueprint")]
		public ScriptableObjects.Blueprint m_blueprint;

		[Tooltip("The sprite when the blueprint is locked")]
		public Sprite m_lockedSprite;

	#endregion
	
	#region Methods
		
	// Start is called before the first frame update
	void Start()
	{
		//
		// We ensure the blueprint is set
		Assert.IsNotNull(m_blueprint, "A blueprint in the inventory has not blueprint data ! (" + transform.name + ")");
		
		//
		// Set the blueprint icon
		SetIcon();
	}

	// Update is called once per frame
	void Update()
	{
		//
		// Set the blueprint icon
		SetIcon();
	}
	
	// Private

	/// <summary>
	/// Set the icon of the blueprint in the inventory. Sets a lock sprite if the blueprint has not been discovered
	/// </summary>
	private void SetIcon()
	{
		//
		// Set the blueprint icon
		if (null != m_blueprint)
		{
			// TODO: Optimize this since it is called in a critical section (update)
			//
			// If the blueprint is not available at the beginning and id it is not unlocked
			// in the party inventory, we put the lock sprite instead of the blueprint icon
			if (m_blueprint.StartLocked && !PartyInventory.Instance.IsBlueprintUnlocked(m_blueprint))
			{
				GetComponent<Image>().sprite = m_lockedSprite;	// display the locked sprite
			}
			else
			{
				GetComponent<Image>().sprite = m_blueprint.GetIcon();	// display the blueprint icon
			}
		}
	}
	
	#endregion
}
