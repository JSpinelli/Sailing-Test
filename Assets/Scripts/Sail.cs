using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Sail : MonoBehaviour
{

    public float defaultTorque = 1.2f;
    public float magnitudeMultiplier = 0.1f;
    public float SailForce()
    {
        Vector2 sailDirection = new Vector2(gameObject.transform.forward.x, gameObject.transform.forward.z);
        float dot = Vector2.Dot(sailDirection.normalized, WindManager.instance.wind.normalized);
        return Math.Abs(dot);
    }

    public float TorqueForce()
    {
        Vector2 sailDirection = new Vector2(gameObject.transform.right.x, gameObject.transform.right.z);
        Vector2 sailPosition = new Vector2(gameObject.transform.right.x, gameObject.transform.right.y);
        float dot = Vector2.Dot(sailDirection.normalized, WindManager.instance.wind.normalized);
        float dotPos = Vector2.Dot(sailPosition.normalized, Vector2.up);
        if ((dotPos <= 0 && dot <= 0) || (dotPos >= 0 && dot >= 0))
        {
            return WindManager.instance.wind.magnitude * magnitudeMultiplier * dot * defaultTorque;
        }
        return WindManager.instance.wind.magnitude * magnitudeMultiplier * dot * (1.2f - Mathf.Abs(dotPos));
    }
}