/**
 *  Author : Kranck
 *  Date : 2020/10/03
 *  Description : Base item scriptable object
 **/

using UnityEngine;

namespace ScriptableObjects
{
	[CreateAssetMenu(menuName = "La Pantera/Items/Create new item", fileName = "New Item", order = 1)]
	public class Item : ScriptableObject
	{

		[Header("General")] 
		[Tooltip("The name of the item")]
		public string m_name;

		[Tooltip("The description of the item that is displayed")]
		public string m_description;

		[Header("Visual")]
		[Tooltip("The icon of the item")]
		public Sprite m_icon;

		[Tooltip("The \"in-game\" representation of the item : the prefab of the item")]
		public GameObject m_prefab;

	}
}

