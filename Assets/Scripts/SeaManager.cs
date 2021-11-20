using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaManager : MonoBehaviour
{
    private GameObject[] tiles;

    public GameObject boat;

    private float xOffset;
    private float zOffset;

    private void Update()
    {
        Vector3 newPos = new Vector3(boat.transform.position.x, 0, boat.transform.position.z);
        transform.position = newPos;
    }
}
