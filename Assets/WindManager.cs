using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    public static WindManager instance;

    public Vector2 wind;

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

    public Vector2 GetWind(float x)
    {
        return wind;
    }
}
