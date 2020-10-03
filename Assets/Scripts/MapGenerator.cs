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
        public Biome biome;
        
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

    public int specialObjectsMinDistanceFromSpawn = 200;

    public int yHeight = -3;

    private Vector2 _realSize;
    private Vector2 _middlePoint;

    [Tooltip("Sum of probabilities must be equal to one")] 
    public List<BiomeInfo> biomeList;

    [Tooltip("Objects that must be spawned in the map")]
    public List<GameObject> specialObjects;
    
    // Start is called before the first frame update
    void Start()
    {
        _realSize = new Vector2(mapXSize * tileXScale, mapZSize * tileZScale);
        _middlePoint = _realSize / 2.0f;
        
        MapBiomeToProbabilityRanges();
        GenerateMap();
        PlaceSpecialObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetRealSize()
    {
        return _realSize;
    }

    public Vector2 GetMiddlePointWorld()
    {
        return transform.position + new Vector3(_middlePoint.x, yHeight, _middlePoint.y);
    }

    [Server]
    private void GenerateMap()
    {
        for (int currentX = 0; currentX < mapXSize * tileXScale; currentX += tileXScale)
        {
            for (int currentZ = 0; currentZ < mapZSize * tileZScale; currentZ += tileZScale)
            {
                BiomeInfo biomeToSpawnInfo = GetRandomBiomeInfo();
                
                GameObject tile = Instantiate(biomeToSpawnInfo.biome.gameObject, new Vector3(currentX, yHeight, currentZ), Quaternion.identity);
                tile.transform.localScale = new Vector3(tileXScale, 1, tileZScale);
                
                tile.transform.SetParent(this.transform);

                NetworkServer.Spawn(tile);
                
                GenerateBiome(biomeToSpawnInfo.biome, new Vector2(currentX, currentZ));
            }
        }
    }

    [Server]
    private void GenerateBiome(Biome biome, Vector2 startPos)
    {
        Vector2 endPos = new Vector2(startPos.x + tileXScale, startPos.y + tileZScale);

        foreach (Biome.ObjectInfo objectInfo in biome.objectList)
        {
            int objectToSpawnCount = Random.Range(objectInfo.spawnMinCount, objectInfo.spawnMaxCount);

            for (int objectIndex = 0; objectIndex < objectToSpawnCount; ++objectIndex)
            {
                PlaceObject(objectInfo.objectPrefab, startPos, endPos);
            }
        }
    }

    [Server]
    private void PlaceObject(GameObject gameObject, Vector2 startPos, Vector2 endPos)
    {
        Vector3 objectPos = new Vector3();
        objectPos.x = Random.Range(startPos.x, endPos.x);
        objectPos.y = yHeight;
        objectPos.z = Random.Range(startPos.y, endPos.y);

        GameObject spawnedObject = Instantiate(gameObject, objectPos, Quaternion.identity);
        spawnedObject.transform.SetParent(this.transform);

        NetworkServer.Spawn(spawnedObject);
    }

    [Server]
    private void PlaceSpecialObjects()
    {
        Vector4[] spawnBoxes = new Vector4[4];
        spawnBoxes[0] = new Vector4(0, 0, _middlePoint.x - specialObjectsMinDistanceFromSpawn, _realSize.y);
        spawnBoxes[1] = new Vector4(_middlePoint.x + specialObjectsMinDistanceFromSpawn, 0, _realSize.x, _realSize.y);
        spawnBoxes[2] = new Vector4(0, 0, _realSize.x, _middlePoint.y - specialObjectsMinDistanceFromSpawn);
        spawnBoxes[3] = new Vector4(0, _middlePoint.y + specialObjectsMinDistanceFromSpawn, _realSize.x, _realSize.y);

        foreach (GameObject specialObject in specialObjects)
        {
            Vector4 selectedBox = spawnBoxes[Random.Range(0, 3)];
            
            Vector3 objectPos = new Vector3();
            objectPos.x = Random.Range(selectedBox.x, selectedBox.z);
            objectPos.y = yHeight;
            objectPos.z = Random.Range(selectedBox.y, selectedBox.w);

            GameObject spawnedObject = Instantiate(specialObject, objectPos, Quaternion.identity);
            spawnedObject.transform.SetParent(this.transform);
            
            NetworkServer.Spawn(spawnedObject);
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
