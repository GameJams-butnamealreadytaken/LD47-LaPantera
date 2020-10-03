/**
 *  Author : Kranck
 *  Date : 2020/10/03
 *  Description : Base blueprint (craft) scriptable object
 **/

using System;
using UnityEngine;

namespace ScriptableObjects
{
	[CreateAssetMenu(menuName = "La Pantera/Blueprints/Create new blueprint", fileName = "New Blueprint", order = 2)]
	public class Blueprint : ScriptableObject
	{
		
	#region Variables
	
		/// <summary>
		/// A recipe element represent a resource we need for a blueprint's recipe
		/// Each element consists of an item and a quantity needed
		/// </summary>
		[Serializable]
		public class RecipeElement
		{
			public Item m_item;		//< The item for this element
			public int m_quantity;	//< The amount of this item needed to satisfy this element
		}
	
		[Header("General")] 
		[Tooltip("The name of the blueprint")]
		public string m_name;
	
		[Header("Visual")]
		[Tooltip("The icon of the blueprint. If nothing is set, the sprite used is the one of the produced item")]
		public Sprite m_icon;
	
		[Header("Recipe")] 
		[Tooltip("The elements needed for the recipe")]
		public RecipeElement[] m_recipeElements;
	
		[Header("Product")] 
		[Tooltip("The produced item")]
		public Item m_produced;
	
	#endregion
	
	#region Methods
	
		/// <summary>
		/// Return the icon of this blueprint
		/// </summary>
		/// <returns>The icon of this blueprint. If the blueprint has no icon, the icon is the one of the produced item</returns>
		public Sprite GetIcon()
		{
			if (null == m_icon)
			{
				return m_produced.m_icon;
			}
			else
			{
				return m_icon;
			}
		}
	
	#endregion
	
	}
}
