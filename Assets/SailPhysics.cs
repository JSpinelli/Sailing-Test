using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SailPhysics : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 sailDirection = new Vector2(gameObject.transform.forward.x, gameObject.transform.forward.z);
        //float dot = Vector2.Dot(sailDirection.normalized, WindManager.instance.wind.normalized);
        //rb.AddRelativeForce(Vector3.left * ((1-Math.Abs(dot)) * WindManager.instance.windMagnitude));
        rb.AddForce(new Vector3(WindManager.instance.wind.x,0,WindManager.instance.wind.y));
    }
}
