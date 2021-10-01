using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ManualSailPhysics : MonoBehaviour
{
    // Update is called once per frame
    public GameObject mast;

    public Transform forward1;
    public Transform forward2;
    public Transform right1;
    public Transform right2;

    [Range(0.0001f,0.15f)]
    public float rope;
    void Update()
    {
        Vector3 right = right2.position - right1.position;
        Vector3 forward = forward2.position - forward1.position;
        Vector2 sailDirectionRight = new Vector2(right.x, right.z);
        Vector2 sailDirectionForward = new Vector2(forward.x, forward.z);
        
        float dotRight = Vector2.Dot(sailDirectionRight.normalized, WindManager.instance.wind.normalized);
        float dotForward = Vector2.Dot(sailDirectionForward.normalized, WindManager.instance.wind.normalized);
        
        transform.RotateAround(mast.transform.position, Vector3.up, -WindManager.instance.windMagnitude * dotRight * Time.deltaTime);
        transform.RotateAround(mast.transform.position, Vector3.up, -WindManager.instance.windMagnitude * dotRight * Time.deltaTime);
        float rad = transform.rotation.y * Mathf.Deg2Rad * 10;

        if (rad > rope || rad < -rope)
        {
            transform.RotateAround(mast.transform.position, Vector3.up, 1.5f * WindManager.instance.windMagnitude * dotRight * Time.deltaTime);
            transform.RotateAround(mast.transform.position, Vector3.up, 1.5f * WindManager.instance.windMagnitude * dotRight * Time.deltaTime);
        }


    }
}
