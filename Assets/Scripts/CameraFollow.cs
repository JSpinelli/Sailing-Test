using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CameraFollow : MonoBehaviour
{

    private Transform cameraPivot;
    public Transform cameraBase;
    public Transform cameraHead;

    public float lookSensitivity;
    public Transform objective;

    float orbitAngle;
    float pitchAngle;
    float tForHeight;

    public float mindDist;
    public float maxDist;
    public float minHeight;
    public float maxHeight;

    public float heightOffset;

    private float tempMinHeight;
    public float deathZone;

    public float minFOV;
    public float maxFOV;
    
    public Camera cam;

    private void Awake()
    {
        cameraPivot = transform;
    }

    void Update()
    {
        
        //Moving Camera with right click hold with added deadZone to avoid involuntary movement
        // if (Input.GetMouseButton(1))
        // {
        //     // Read the mouse input axis
        //     if (Mathf.Abs(Input.GetAxis("Mouse X")) > deathZone)
        //         orbitAngle += Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
        //     if (Mathf.Abs(Input.GetAxis("Mouse Y")) > deathZone)
        //         pitchAngle -= Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;
        // }
        pitchAngle = Mathf.Clamp(pitchAngle, -25, 50);
        
        pitchAngle = Mathf.Lerp(pitchAngle, 10, Time.deltaTime * 2);
        orbitAngle = Mathf.Lerp(orbitAngle, 0, Time.deltaTime);

            //Adjust height and FOV when adjusting pitch 
        if (pitchAngle >= 0)
        {
            tForHeight = (pitchAngle / 50) + heightOffset;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, minFOV, Time.deltaTime * 2);
        }
        else
        {
            tForHeight = (1 - (Mathf.Abs(pitchAngle) / 25)) * heightOffset;
            cam.fieldOfView = Mathf.Lerp(minFOV, maxFOV, (Mathf.Abs(pitchAngle) / 25));
        }
        
        //Camera Pivot Movement
        cameraPivot.localRotation = Quaternion.Euler(0, orbitAngle, 0);
        //cameraPivot.position
        
        //Camera Base Movement
        //cameraBase.localRotation
        cameraBase.localPosition = -Vector3.forward * Mathf.Lerp(mindDist, maxDist, tForHeight);
        
        transform.position = Vector3.Lerp(transform.position, objective.position, Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, objective.rotation,3 );
        
        
        //Camera Head Movement
        cameraHead.localRotation = Quaternion.Euler(pitchAngle, 0, 0);
        cameraHead.localPosition = Vector3.up * Mathf.Lerp(minHeight, maxHeight, tForHeight);
    }
}
