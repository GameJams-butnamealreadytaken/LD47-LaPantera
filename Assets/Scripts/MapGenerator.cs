using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class MapGenerator : NetworkBehaviour
{
    [Serializable]
    public class BiomeInfo
    {
        public GameObject tilePrefab;
        
        [Range(0.0f, 1.0f)]
        public float spawnProbability;

        [NonSerialized]
        public float spawnProbabilityRangeBegin;
        
        [NonSerialized]
        public float spawnProbabilityRangeEnd;
    }

    public int tileXScale = 20;
    public int tileZScale = 20;

    public int mapXSize = 20;
    public int mapZSize = 20;

    public int zHeight = -3;

    public List<BiomeInfo> biomeList; 
    
    // Start is called before the first frame update
    void Start()
    {
        MapBiomeToProbabilityRanges();
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
                GameObject tile = Instantiate(GetRandomBiomeInfo().tilePrefab, new Vector3(currentX, zHeight, currentZ), Quaternion.identity);
                tile.transform.localScale = new Vector3(tileXScale, 1, tileZScale);
                
                NetworkServer.Spawn(tile);
            }
        }
    }

    [Server]
    private void MapBiomeToProbabilityRanges()
    {
        biomeList.Sort((lhs, rhs) => (lhs.spawnProbability.CompareTo(rhs.spawnProbability)));

        float currentProbabilityRangeBegin = 0.0f;
        foreach (BiomeInfo biomeInfo in biomeList)
        {
            biomeInfo.spawnProbabilityRangeBegin = currentProbabilityRangeBegin;
            biomeInfo.spawnProbabilityRangeEnd = currentProbabilityRangeBegin + biomeInfo.spawnProbability;

            currentProbabilityRangeBegin += biomeInfo.spawnProbability;
        }
    }

    [Server]
    private BiomeInfo GetRandomBiomeInfo()
    {
        float randomNumber = Random.Range(0.0f, 1.0f);

        foreach (BiomeInfo biomeInfo in biomeList)
        {
            if (randomNumber >= biomeInfo.spawnProbabilityRangeBegin &&
                randomNumber < biomeInfo.spawnProbabilityRangeEnd)
            {
                return biomeInfo;
            }
        }

        Assert.IsTrue(false, "Should never happen");
        return new BiomeInfo();
    }
}
