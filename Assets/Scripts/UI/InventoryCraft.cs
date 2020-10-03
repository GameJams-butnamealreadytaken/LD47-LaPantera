/**
 *  Author : Kranck
 *  Date : 2020/10/03
 *  Description : Manages the craft inventory
 **/

using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryCraft : MonoBehaviour
{
	#region Variables
	
		[Tooltip("The text that is used to display the description of the item to craft")]
		public TMP_Text m_descriptionText;

	#endregion
	
	#region Methods
	
		// Start is called before the first frame update
		void Start()
		{
			
		}

		// Update is called once per frame
		void Update()
		{
			//
			//
			ScriptableObjects.Blueprint selectedBlueprint = EventSystem.current.currentSelectedGameObject
				.GetComponentInChildren<InventoryBlueprint>().m_blueprint;
			
			//
			// update the description text depending on the current selected blueprint
			// If the blueprint is not unlocked, we simply display "???", otherwise we display the description
			// of the blueprint (which in fact is the description of the produced item)
			if (selectedBlueprint.StartLocked && !PartyInventory.Instance.IsBlueprintUnlocked(selectedBlueprint))
			{
				m_descriptionText.text = "???";
			}
			else
			{
				m_descriptionText.text = EventSystem.current.currentSelectedGameObject
					.GetComponentInChildren<InventoryBlueprint>().m_blueprint.Description;
			}
		}
	
	#endregion 
}
