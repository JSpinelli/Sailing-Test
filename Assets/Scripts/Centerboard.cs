using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centerboard : MonoBehaviour
{
    public Rigidbody rigidBody;
    public Transform tillerOrigin;
    public Transform tillerDest;
    private void FixedUpdate()
    {
        rigidBody.AddForce(-Vector3.Project(rigidBody.velocity, transform.forward));
        //rigidBody.AddForce(-Vector3.Project(rigidBody.velocity, tillerOrigin.position - tillerDest.position));
        rigidBody.AddTorque(-Vector3.Project(rigidBody.angularVelocity, Vector3.up));
    }
}
