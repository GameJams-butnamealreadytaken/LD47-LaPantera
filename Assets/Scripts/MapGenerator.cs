using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MapGenerator : NetworkBehaviour
{
    [Serializable]
    public struct BiomeInfo
    {
        public GameObject tilePrefab;
        
        [Range(0.0f, 1.0f)]
        public float spawnProbability;
    }
    
    public int tileXScale = 20;
    public int tileZScale = 20;

    public int mapXSize = 20;
    public int mapZSize = 20;

    public List<BiomeInfo> biomeList; 
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Server]
    private void GenerateMap()
    {
        for (int currentX = 0; currentX < mapXSize * tileXScale; currentX += tileXScale)
        {
            for (int currentZ = 0; currentZ < mapZSize * tileZScale; currentZ += tileZScale)
            {
                GameObject tile = Instantiate(biomeList[0].tilePrefab, new Vector3(currentX, 0, currentZ), Quaternion.identity);
                tile.transform.localScale = new Vector3(tileXScale, 1, tileZScale);
                
                NetworkServer.Spawn(tile);
            }
        }
    }
}
