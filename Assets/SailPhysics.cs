using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SailPhysics : MonoBehaviour
{
    private Rigidbody rb;

    public Rigidbody ship;

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
        Vector3 dir = new Vector3(WindManager.instance.wind.x, 0, WindManager.instance.wind.y);
        Debug.Log(dir);
        Vector3 pos1 = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z - 0.5f);
        Vector3 pos2 = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z + 0.5f);
        Vector3 pos3 = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z - 0.5f);
        Vector3 pos4 = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z + 0.5f);
        rb.AddForceAtPosition(dir / 4, pos1, ForceMode.Force);
        rb.AddForceAtPosition(dir / 4, pos2, ForceMode.Force);
        rb.AddForceAtPosition(dir / 4, pos3, ForceMode.Force);
        rb.AddForceAtPosition(dir / 4, pos4, ForceMode.Force);
    }
}