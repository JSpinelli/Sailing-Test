using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centerboard : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public float waterAngularDrag = 0.5f;
    public float offset = 0f;
    private void FixedUpdate()
    {
        // float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
        // //float waveHeight = WavesGenerator.instance.GetWaterHeight(transform.position);
        // if (transform.position.y + offset < waveHeight)
        // {
        //     float displacementMultiplier =
        //         Mathf.Clamp01((waveHeight-transform.position.y) / depthBeforeSubmerged) * displacementAmount;
        //     rigidBody.AddTorque(-rigidBody.angularVelocity * (displacementMultiplier * waterAngularDrag * Time.fixedDeltaTime),ForceMode.VelocityChange);
        // }
        rigidBody.AddForce(-Vector3.Project(rigidBody.velocity, transform.forward));
        rigidBody.AddTorque(Vector3.Project(rigidBody.angularVelocity, Vector3.up));
        
        // rigidBody.AddForce(-Vector3.Project(rigidBody.velocity, transform.forward) * Time.fixedDeltaTime);
        // rigidBody.AddTorque(-Vector3.Project(rigidBody.angularVelocity, Vector3.up) * Time.fixedDeltaTime);
        
        //rigidBody.velocity = Vector3.Project(rigidBody.velocity, transform.up);
        //rigidBody.velocity = Vector3.Project(rigidBody.velocity, transform.right);
    }
}
