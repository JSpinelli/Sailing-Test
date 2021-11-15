using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OceanSpawner : MonoBehaviour
{
    private List<GameObject> myPool;
    private int activeObjects;
    public GameObject prefab;
    private int startingNumber;

    public Transform playerPos;
    private Vector3 playerCoord;
    public int spawningRange;
    private Dictionary<string, GameObject> myRange;
    private Vector3 previousPos;

    public int worldX;
    public int worldZ;

    private int worldXOffset;
    private int worldZOffset;
    private mapData currentData;

    public struct mapData
    {
        public Vector2 windValue;
        public float waveAmplitude;
        public float waveLength;
        public GameObject obj;
    }

    private mapData[,] world;

    public void Start() //or Awake??
    {
        startingNumber = spawningRange * spawningRange;
        activeObjects = 0;
        myPool = new List<GameObject>();
        gameObject.transform.position = Vector3.zero;

        playerCoord = playerPos.position / 10;
        previousPos = playerPos.position / 10;
        worldXOffset = worldX / 2;
        worldZOffset = worldZ / 2;
        world = new mapData[worldX, worldZ];
        for (int x = 0; x < worldX; x++)
        {
            for (int z = 0; z < worldZ; z++)
            {
                mapData mp = new mapData();
                mp.windValue = new Vector2(Random.Range(0, 20), Random.Range(0, 20));
                mp.waveAmplitude = 4; //Random.Range(-10, 10);
                mp.waveLength = 4;//Random.Range(5, 5);
                world[x, z] = mp;
            }
        }

        for (int i = (int) playerCoord.x / 10 - spawningRange; i < playerCoord.x / 10 + spawningRange; i++)
        {
            for (int j = (int) playerCoord.y / 10 - spawningRange; j < playerCoord.y / 10 + spawningRange; j++)
            {
                world[i + worldXOffset, j + worldZOffset].obj = Instantiate(prefab, new Vector3(i * 10, 0, j * 10),
                    Quaternion.identity,
                    gameObject.transform);
                myPool.Add(world[i + worldXOffset, j + worldZOffset].obj);
            }
        }

        currentData = CurrentMapData();
    }

    public mapData CurrentMapData()
    {
        int x = (int) playerCoord.x / 10;
        int z = (int) playerCoord.z / 10;
        return world[x + worldXOffset, z + worldZOffset];
    }

    private void Update()
    {
        playerCoord = playerPos.position / 10;
        float xDiff = previousPos.x - playerCoord.x;
        float zDiff = previousPos.z - playerCoord.z;
        int rangeX = (int) (Mathf.Floor(Mathf.Abs(xDiff)) * -Mathf.Sign(xDiff));
        int rangeZ = (int) (Mathf.Floor(Mathf.Abs(zDiff)) * -Mathf.Sign(zDiff));
        // --- TOP OR BOTTOM
        if (Mathf.Abs(rangeZ) >= 1)
        {
            // DESPAWN
            for (int x = (int) previousPos.x - spawningRange; x < previousPos.x + spawningRange; x++)
            {
                int zBeggining = (int) previousPos.z - (int) (Mathf.Sign(rangeZ) * spawningRange) + rangeZ;
                int zEnd = (int) previousPos.z - (int) (Mathf.Sign(rangeZ) * spawningRange);
                if (rangeZ > 0)
                {
                    for (int z = zEnd; z < zBeggining; z++)
                    {
                        Destroy(world[x + worldXOffset, z + worldZOffset].obj);
                    }
                }
                else
                {
                    for (int z = zBeggining; z < zEnd; z++)
                    {
                        Destroy(world[x + worldXOffset, z + worldZOffset].obj);
                    }
                }
            }

            // SPAWN
            for (int x = (int) previousPos.x - spawningRange; x < previousPos.x + spawningRange; x++)
            {
                int zBeggining = (int) previousPos.z + (int) (Mathf.Sign(rangeZ) * spawningRange) + rangeZ;
                int zEnd = (int) previousPos.z + (int) (Mathf.Sign(rangeZ) * spawningRange);
                if (rangeZ > 0)
                {
                    for (int z = zEnd; z < zBeggining; z++)
                    {
                        world[x + worldXOffset, z + worldZOffset].obj = Instantiate(new Vector3(x * 10, 0, z * 10));
                        myPool.Add(world[x + worldXOffset, z + worldZOffset].obj);
                    }
                }
                else
                {
                    for (int z = zBeggining; z < zEnd; z++)
                    {
                        world[x + worldXOffset, z + worldZOffset].obj = Instantiate(new Vector3(x * 10, 0, z * 10));
                        myPool.Add(world[x + worldXOffset, z + worldZOffset].obj);
                    }
                }
            }

            previousPos.z = previousPos.z + rangeZ;
        }

        // --- LEFT OR RIGHT
        if (Mathf.Abs(rangeX) >= 1)
        {
            // DESPAWN
            int xBeggining = (int) previousPos.x - (int) (Mathf.Sign(rangeX) * spawningRange) + rangeX;
            int xEnd = (int) previousPos.x - (int) (Mathf.Sign(rangeX) * spawningRange);
            if (rangeX > 0)
            {
                for (int x = xEnd; x < xBeggining; x++)
                {
                    for (int z = (int) previousPos.z - spawningRange; z < previousPos.z + spawningRange; z++)
                    {
                        Destroy(world[x + worldXOffset, z + worldZOffset].obj);
                    }
                }
            }
            else
            {
                for (int x = xBeggining; x < xEnd; x++)
                {
                    for (int z = (int) previousPos.z - spawningRange; z < previousPos.z + spawningRange; z++)
                    {
                        Destroy(world[x + worldXOffset, z + worldZOffset].obj);
                    }
                }
            }

            // SPAWN
            xBeggining = (int) previousPos.x + (int) (Mathf.Sign(rangeX) * spawningRange) + rangeX;
            xEnd = (int) previousPos.x + (int) (Mathf.Sign(rangeX) * spawningRange);
            if (rangeX > 0)
            {
                for (int x = xEnd; x < xBeggining; x++)
                {
                    for (int z = (int) previousPos.z - spawningRange; z < previousPos.z + spawningRange; z++)
                    {
                        world[x + worldXOffset, z + worldZOffset].obj = Instantiate(new Vector3(x * 10, 0, z * 10));
                        myPool.Add(world[x + worldXOffset, z + worldZOffset].obj);
                    }
                }
            }
            else
            {
                for (int x = xBeggining; x < xEnd; x++)
                {
                    for (int z = (int) previousPos.z - spawningRange; z < previousPos.z + spawningRange; z++)
                    {
                        world[x + worldXOffset, z + worldZOffset].obj = Instantiate(new Vector3(x * 10, 0, z * 10));
                        myPool.Add(world[x + worldXOffset, z + worldZOffset].obj);
                    }
                }
            }

            previousPos.x = previousPos.x + rangeX;
        }

        currentData = CurrentMapData();
        
        // WaveManager.instance.amplitude =
        //     Mathf.Lerp(WaveManager.instance.amplitude, currentData.waveAmplitude, Time.deltaTime);
        // WaveManager.instance.length =
        //     Mathf.Lerp(WaveManager.instance.length, currentData.waveLength, Time.deltaTime);
        
        //WindManager.instance.wind = Vector2.Lerp(WindManager.instance.wind,currentData.windValue,Time.deltaTime);
    }

    public GameObject Instantiate(Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject obj;
        if (activeObjects == myPool.Count)
        {
            obj = Instantiate(prefab, position, rotation, parent);
            myPool.Add(obj);
        }
        else
        {
            obj = myPool.Find((o => { return !o.activeSelf; }));
            obj.SetActive(true);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.transform.parent = parent;
        }

        activeObjects++;
        return obj;
    }

    public GameObject Instantiate(Vector3 position, Quaternion rotation)
    {
        GameObject obj;
        if (activeObjects == myPool.Count)
        {
            obj = Instantiate(prefab, position, rotation, gameObject.transform);
            myPool.Add(obj);
        }
        else
        {
            obj = myPool.Find((o => { return !o.activeSelf; }));
            obj.SetActive(true);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
        }

        activeObjects++;
        return obj;
    }

    public GameObject Instantiate(Vector3 position)
    {
        GameObject obj;
        if (activeObjects == myPool.Count)
        {
            obj = Instantiate(prefab, position, Quaternion.identity, gameObject.transform);
            myPool.Add(obj);
        }
        else
        {
            obj = myPool.Find((o => { return !o.activeSelf; }));
            obj.SetActive(true);
            obj.transform.position = position;
        }

        activeObjects++;
        return obj;
    }

    public void Destroy(GameObject obj)
    {
        obj.SetActive(false);
        activeObjects--;
    }

    public void OnDestroy()
    {
        foreach (var obj in myPool)
        {
            Destroy(obj);
        }
    }
}