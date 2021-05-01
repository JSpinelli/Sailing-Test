using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Sail : MonoBehaviour
{
    public Vector3 SailForce()
    {
        Vector2 sailDirection = new Vector2(gameObject.transform.forward.x, gameObject.transform.forward.z);
        float dot = Vector2.Dot(sailDirection, WindManager.instance.wind);
        if (dot < WindManager.instance.noGo)
        {
            Debug.Log("IN NO GO");
            return Vector3.zero;
        }
        return gameObject.transform.forward * (WindManager.instance.wind.magnitude * (1 - Math.Abs(dot)));
    }
}