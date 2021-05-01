using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sail : MonoBehaviour
{
    public Rigidbody boat;
    public Transform mastContact;

    private void FixedUpdate()
    {
        Vector2 sailDirection = new Vector2(gameObject.transform.right.x, gameObject.transform.right.z);
        if (Vector2.Dot(sailDirection, WindManager.instance.wind) > 0.5)
        {
            boat.AddForceAtPosition(gameObject.transform.right * WindManager.instance.wind.magnitude * 0.1f,
                mastContact.position, ForceMode.Impulse);
        }
    }
}
