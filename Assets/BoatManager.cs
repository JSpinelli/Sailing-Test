using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatManager : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Sail mainSail;
    public Sail frontSail;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Rotate(0, -1 * _rigidbody.velocity.magnitude, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Rotate(0, 1 * _rigidbody.velocity.magnitude, 0);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(
            gameObject.transform.forward +
            mainSail.SailForce() +
            frontSail.SailForce() * 0.5f
        );
    }
}