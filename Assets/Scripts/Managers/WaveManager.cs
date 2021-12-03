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

    public float lerpingSpeed = 0.2f;
    
    private float _targetAmpX;
    private float _targetAmpZ;
    private float _targetLenghtX;
    private float _targetLenghtZ;

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
        if (_targetAmpX != amplitudeX)
        {
            amplitudeX = Mathf.Lerp(amplitudeX, _targetAmpX, Time.deltaTime * lerpingSpeed);
        }
        if (_targetAmpZ != amplitudeZ)
        {
            amplitudeZ = Mathf.Lerp(amplitudeZ, _targetAmpZ, Time.deltaTime * lerpingSpeed);
        }
        if (_targetLenghtX != lengthX)
        {
            lengthX = Mathf.Lerp(lengthX, _targetLenghtX, Time.deltaTime * lerpingSpeed);
        }
        if (_targetLenghtZ != lengthZ)
        {
            lengthZ = Mathf.Lerp(lengthZ, _targetLenghtZ, Time.deltaTime * lerpingSpeed);
        }
    }

    public float GetWaveHeight(float x, float z)
    {
        return (amplitudeX * Mathf.Sin(x / lengthX + offsetX)) + amplitudeZ * Mathf.Sin(z / lengthZ + offsetZ);
    }

    public void ChangeWaveValues(float ampX,float lenX, float ampZ, float lenZ)
    {
        _targetAmpX = ampX;
        _targetAmpZ = ampZ;
        _targetLenghtX = lenX;
        _targetLenghtZ = lenZ;
    }
}
