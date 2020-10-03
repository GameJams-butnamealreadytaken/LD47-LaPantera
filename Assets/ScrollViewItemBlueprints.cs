using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ScrollViewItemBlueprints : MonoBehaviour
{

	public RectTransform m_content;

	public Transform m_spawnPoint;

	public GameObject m_item;

	public ScrollRect m_scrollRect;
	// Start is called before the first frame update
	void Start()
	{
		//setContent Holder Height;
		m_content.sizeDelta = new Vector2(0, 28 * 80);     

		for (int i = 0; i < 28; i++)
		{
		    // 60 width of item
		    float spawnY = i * 80;
		    //newSpawn Position
		    Vector3 pos = new Vector3(m_spawnPoint.localPosition.x, -spawnY, m_spawnPoint.localPosition.z);
		    //instantiate item
		    GameObject SpawnedItem = Instantiate(m_item, pos, m_spawnPoint.rotation);
		    //setParent
		    SpawnedItem.transform.SetParent(m_spawnPoint, false);
		    //get ItemDetails Component
		    // ItemDetails itemDetails = SpawnedItem.GetComponent<ItemDetails>();
		    // //set name
		    // itemDetails.text.text = itemNames[i];
		    // //set image
		    // itemDetails.image.sprite = itemImages[i];  
		    SpawnedItem.GetComponentInChildren<Text>().text = "Yahou " + i.ToString();


		}
		
		
	}

	
	// Update is called once per frame
	void Update()
	{
		if (EventSystem.current.currentSelectedGameObject)
		{
			RectTransform rt = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>();
			float pp = rt.anchoredPosition.y + m_content.anchoredPosition.y;
			// float pp = rt.anchoredPosition.y + m_content.anchoredPosition.y;
			Debug.Log("PP : " + pp);
			float min = 0f;
			float max = 28 * 80;
			float val = Mathf.Abs(pp);
			// m_scrollRect.verticalNormalizedPosition = pp;
			m_scrollRect.verticalNormalizedPosition = 1.0f - (Mathf.Abs(rt.anchoredPosition.y) / max);
		}
	}
}
