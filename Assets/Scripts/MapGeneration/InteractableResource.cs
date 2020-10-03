using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class InteractableResource : NetworkBehaviour
{
    public ScriptableObjects.Item resourceDescriptor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Gather()
    {
        List<GameObject> spawnedObjects = resourceDescriptor.Spawn(this.transform.position, 1);

        foreach (GameObject spawnedObject in spawnedObjects)
        {
            NetworkServer.Spawn(spawnedObject);
        }
        
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            Debug.Log("YES IT IS ");
        }
        
        Item item = other.gameObject.GetComponent<Item>();

        if (item == null)
        {
            return;
        }
        
        Gather();
    }
}
