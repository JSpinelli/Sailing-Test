using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public float amplitudeX = 1f;
    public float lengthX = 2f;
    public float speedX = 1f;
    private float offsetX = 0f;    
    
    public float amplitudeZ = 1f;
    public float lengthZ = 2f;
    public float speedZ = 1f;
    private float offsetZ = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Should not be another class");
            Destroy(this);
        }
    }

    private void Update()
    {
        offsetX += Time.deltaTime * speedX;
        offsetZ += Time.deltaTime * speedZ;
    }

    public float GetWaveHeight(float x, float z)
    {
        return (amplitudeX * Mathf.Sin(x / lengthX + offsetX)) + amplitudeZ * Mathf.Sin(z / lengthZ + offsetZ);
    }
}
