using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaManager : MonoBehaviour
{
    public GameObject meshTemplate;

    public int xAmountOfTiles = 4;
    public int zAmountOfTiles = 4;

    public int xSize = 20;
    public int zSize = 20;
    
    private GameObject[] tiles;

    public GameObject boat;

    private float xOffset;
    private float zOffset;
    
    private void Start()
    {
        xOffset = xSize * (xAmountOfTiles/2);
        zOffset = zSize * (zAmountOfTiles/2);
        tiles = new GameObject[(zAmountOfTiles ) * (xAmountOfTiles)];
        for (int i = 0, z = 0; z < zAmountOfTiles; z++)
        {
            for (int x = 0; x < xAmountOfTiles; x++)
            {
                tiles[i] = Instantiate(meshTemplate, new Vector3(z * zSize, 0f, x * xSize), Quaternion.identity,transform);
                i++;
            }
        }
    }

    private void Update()
    {
        Vector3 newPos = new Vector3(boat.transform.position.x - xOffset, 0, boat.transform.position.z - zOffset);
        transform.position = newPos;
    }
}
