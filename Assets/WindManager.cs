using System;
using System.ComponentModel;
using UnityEngine;

[ExecuteInEditMode]
public class WindManager : MonoBehaviour
{
    public static WindManager instance;

    public Vector2 wind;
    public float windMagnitude;
    
    public float noGo = -0.45f;
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
        windMagnitude = wind.magnitude;
    }

    public Vector2 GetWind(float x)
    {
        return wind;
    }
}
