using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public float xLimits;

    public float zMin;

    public float zMax;
    
    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.localPosition;
        
        if (transform.localPosition.x <= xLimits && transform.localPosition.x >= -xLimits && PlayerController.cameraDir.x!=0)
        {
            pos.x += PlayerController.cameraDir.x/10;
            pos.x = Mathf.Clamp(pos.x, -xLimits, xLimits);
        }

        if (transform.localPosition.z >= zMin && transform.localPosition.z <= zMax && PlayerController.cameraDir.y!=0)
        {
            pos.z += PlayerController.cameraDir.y/10;
            pos.z = Mathf.Clamp(pos.z, zMin, zMax);
        }
        transform.localPosition = pos;
    }
}
