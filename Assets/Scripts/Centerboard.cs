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
        rigidBody.velocity = Vector3.Project(rigidBody.velocity, transform.forward);
        rigidBody.angularVelocity = new Vector3(
            rigidBody.angularVelocity.x, 
            rigidBody.angularVelocity.y * 0f,
            rigidBody.angularVelocity.z);
        //rigidBody.velocity = Vector3.Project(rigidBody.velocity, transform.up);
        //rigidBody.velocity = Vector3.Project(rigidBody.velocity, transform.right);
    }
}
