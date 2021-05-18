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
        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
        if (transform.position.y + offset < waveHeight)
        {
            float displacementMultiplier =
                Mathf.Clamp01((waveHeight-transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rigidBody.AddTorque(-rigidBody.angularVelocity * (displacementMultiplier * waterAngularDrag * Time.fixedDeltaTime),ForceMode.VelocityChange);
        }
    }
}
