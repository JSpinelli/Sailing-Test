using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public float xLimits;

    public float zMin;

    public float zMax;

    private Vector2 camMovement;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.localPosition;
        
        if (transform.localPosition.x <= xLimits && transform.localPosition.x >= -xLimits && camMovement.x!=0)
        {
            pos.x += camMovement.x/10;
            pos.x = Mathf.Clamp(pos.x, -xLimits, xLimits);
        }

        if (transform.localPosition.z >= zMin && transform.localPosition.z <= zMax && camMovement.y!=0)
        {
            pos.z += camMovement.y/10;
            pos.z = Mathf.Clamp(pos.z, zMin, zMax);
        }
        transform.localPosition = pos;
    }

    public void CamMovement(InputAction.CallbackContext cx)
    {
        
        camMovement = (Vector2) cx.ReadValueAsObject();
    }
}
