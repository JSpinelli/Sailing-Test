using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform cameraPivot;
    public Transform cameraBase;
    public Transform cameraHead;

    public float lookSensitivity;
    public float walkSpeed;

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
    public Camera cam;
    public Transform boat;

    private void Awake()
    {
        cameraPivot = transform;
    }

    void Update()
    {
        //Moving Camera with right click hold with added deadZone to avoid involuntary movement
        // Read the mouse input axis
        if (Math.Abs(PlayerController.cameraDir.x) > deathZone)
            orbitAngle += PlayerController.cameraDir.x * lookSensitivity * Time.deltaTime;
        if (Math.Abs(PlayerController.cameraDir.y) > deathZone)
            pitchAngle -= PlayerController.cameraDir.y * lookSensitivity * Time.deltaTime;

        pitchAngle = Mathf.Clamp(pitchAngle, -45, 40);

        //Adjust height and FOV when adjusting pitch 
        if (pitchAngle >= 0)
        {
            tForHeight = (pitchAngle / 50) + heightOffset;
        }
        else
        {
            tForHeight = (1 - (Mathf.Abs(pitchAngle) / 25)) * heightOffset;
        }

        cameraPivot.localRotation = Quaternion.Euler(0, orbitAngle, 0);

        cameraBase.localPosition = -Vector3.forward * Mathf.Lerp(mindDist, maxDist, tForHeight);

        cameraHead.localRotation = Quaternion.Euler(pitchAngle, 0, 0);
        cameraHead.localPosition = Vector3.up * Mathf.Lerp(minHeight, maxHeight, tForHeight);
        
        Vector3 movement = Quaternion.Euler(0, cam.transform.eulerAngles.y - boat.transform.eulerAngles.y, 0) * new Vector3(PlayerController.playerDir.x * walkSpeed * Time.deltaTime, 0, PlayerController.playerDir.y * walkSpeed * Time.deltaTime);
        movement = transform.localPosition + movement;
        movement.x = Mathf.Clamp(movement.x, -2, 2);
        movement.z = Mathf.Clamp(movement.z, -7, 7);
        transform.localPosition = movement;

    }
}