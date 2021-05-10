using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatManager : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Sail mainSail;
    public Sail frontSail;
    public bool torqueEnabled = true;
    public float torqueMultiplier = 10;

    public bool noSails = false;

    public Text mainSailDisplay;
    public Text frontSailDisplay;

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
        if (noSails)
        {
            _rigidbody.AddForce(
                gameObject.transform.forward.normalized
            );
        }
        else
        {
            float mainSailForce = mainSail.SailForce();
            float frontSailForce = frontSail.SailForce();
            mainSailDisplay.text = "Main Sail: " + mainSailForce;
            frontSailDisplay.text = "Front Sail: " + frontSailForce;
            _rigidbody.AddForce(
                gameObject.transform.forward.normalized * //THIS SHOULD CHANGE ACCORDING TO A&D, model the centerboard
                (mainSailForce + (frontSailForce * 0.5f))
            );
            if (torqueEnabled)
            {
                float force = mainSail.TorqueForce() + frontSail.TorqueForce();
                Debug.Log("Torque: " + force);
                _rigidbody.AddRelativeTorque(0, 0,
                    -force * torqueMultiplier
                    , ForceMode.Force);
            }
        }
    }
}