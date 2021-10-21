using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCamera : MonoBehaviour
{
    private Quaternion previousQuaternion;

    public float smoothFactor;
    // Update is called once per frame
    private void Start()
    {
        previousQuaternion = transform.rotation;
    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(previousQuaternion, transform.rotation,smoothFactor);
    }
}
