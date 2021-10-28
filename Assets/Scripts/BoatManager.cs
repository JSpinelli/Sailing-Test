using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BoatManager : MonoBehaviour
{
    private PlayerControls controls;
    private Rigidbody _rigidbody;
    public Sail mainSail;
    public Sail frontSail;
    public bool torqueEnabled = true;
    public float torqueMultiplier = 10;

    public float speedFactor = 50f;

    public TextMeshProUGUI speedText;
    public TextMeshProUGUI mainSailSpeed;
    public TextMeshProUGUI genoaSpeed;
    public TextMeshProUGUI typeOfSailing;

    public TextMeshProUGUI mainSailRope;
    public TextMeshProUGUI leftGenoaRope;
    public TextMeshProUGUI rightGenoaRope;

    public AudioSource ropeTight;
    public AudioSource ropeUnwind;

    public float turningFactor = 0.5f;

    private float currentSpeed = 0;

    private bool leftGenoaLineAttached = true;
    private bool rightGenoaLineAttached = true;

    private bool leftGenoaGrabbed = false;
    private bool rightGenoaGrabbed = false;
    private bool mainSailGrabbed = false;


    public ManualSailPhysics mainsail;
    public ManualSailPhysics genoa;

    private Vector2 dir;

    private Vector2 dirRope;

    public bool mainSailWorking = false;
    public bool frontSailWorking = false;

    public Transform tillerPos;
    public Transform tillerOrigin;

    private float currentTillerPos = 0;

    private Vector3 currentAngle;

    public float tillerSensitivity=2f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        controls = new PlayerControls();
    }

    private void FixedUpdate()
    {
        float mainSailForce = mainSail.SailForce();
        float frontSailForce = frontSail.SailForce();
        Vector2 sailDirection = new Vector2(gameObject.transform.forward.x, gameObject.transform.forward.z);
        float dot = Vector2.Dot(sailDirection.normalized, WindManager.instance.wind.normalized);

        float mainSailContribution = 0;
        float genoaContribution = 0;
        currentSpeed = 0;
        switch (dot)
        {
            case float f when (f <= WindManager.instance.noGo):
                typeOfSailing.text = "In Irons";
                break;
            case float f when (f > WindManager.instance.noGo && f <= -0.7):
                // CLOSE HAUL
                typeOfSailing.text = "Close Hauled";
                if (0.75f <= mainSailForce && mainSailForce <= 0.95f)
                {
                    mainSailContribution = 1 - (Math.Abs(0.85f - mainSailForce) / 0.1f);
                    currentSpeed += mainSailContribution;
                    mainSailWorking = true;
                }
                else
                {
                    mainSailWorking = false;
                }

                if (0.75f <= frontSailForce && frontSailForce <= 0.95f)
                {
                    genoaContribution = 1 - (Math.Abs(0.85f - frontSailForce) / 0.1f);
                    currentSpeed += genoaContribution;
                    frontSailWorking = true;
                }
                else
                {
                    frontSailWorking = false;
                }

                break;
            case float f when (f > -0.7 && f <= -0.1):
                // CLOSE REACH
                typeOfSailing.text = "Close Reach";
                if (0.75f <= mainSailForce && mainSailForce <= 0.95f)
                {
                    mainSailContribution = 1 - (Math.Abs(0.85f - mainSailForce) / 0.1f);
                    currentSpeed += mainSailContribution;
                    mainSailWorking = true;
                }
                else
                {
                    mainSailWorking = false;
                }

                if (0.75f <= frontSailForce && frontSailForce <= 0.95f)
                {
                    genoaContribution = 1 - (Math.Abs(0.85f - frontSailForce) / 0.1f);
                    currentSpeed += genoaContribution;
                    frontSailWorking = true;
                }
                else
                {
                    frontSailWorking = false;
                }

                break;
            case float f when (f > -0.1 && f <= 0.1):
                // BEAM REACH
                typeOfSailing.text = "Beam Reach";
                if (0.3f <= mainSailForce && mainSailForce <= 0.7f)
                {
                    mainSailContribution = 1 - (Math.Abs(0.5f - mainSailForce) / 0.2f);
                    currentSpeed += mainSailContribution;
                    mainSailWorking = true;
                }
                else
                {
                    mainSailWorking = false;
                }

                if (0.3f <= frontSailForce && frontSailForce <= 0.7f)
                {
                    genoaContribution = 1 - (Math.Abs(0.5f - frontSailForce) / 0.2f);
                    currentSpeed += genoaContribution;
                    frontSailWorking = true;
                }
                else
                {
                    frontSailWorking = false;
                }

                break;
            case float f when (f > 0.1 && f <= 0.9):
                //BROAD REACH
                typeOfSailing.text = "Broad Reach";
                if (0.01f <= mainSailForce && mainSailForce <= 0.5f)
                {
                    mainSailContribution = 1 - (Math.Abs(0.25f - mainSailForce) / 0.25f);
                    currentSpeed += mainSailContribution;
                    mainSailWorking = true;
                }
                else
                {
                    mainSailWorking = false;
                }

                if (0.01f <= frontSailForce && frontSailForce <= 0.5f)
                {
                    genoaContribution = 1 - (Math.Abs(0.25f - frontSailForce) / 0.25f);
                    currentSpeed += genoaContribution;
                    frontSailWorking = true;
                }
                else
                {
                    frontSailWorking = false;
                }

                break;
            case float f when (f > 0.9):
                //RUNNING
                typeOfSailing.text = "Running";
                if (0.01f <= mainSailForce && mainSailForce <= 0.4f)
                {
                    mainSailContribution = 1 - (Math.Abs(0.2f - mainSailForce) / 0.2f);
                    currentSpeed += mainSailContribution;
                    mainSailWorking = true;
                }
                else
                {
                    mainSailWorking = false;
                }

                if (0.01f <= frontSailForce && frontSailForce <= 0.4f)
                {
                    genoaContribution = 1 - (Math.Abs(0.2f - frontSailForce) / 0.2f);
                    currentSpeed += genoaContribution;
                    frontSailWorking = true;
                }
                else
                {
                    frontSailWorking = false;
                }

                break;
        }

        mainSailSpeed.text = "Main Sail Force: " + mainSailContribution;
        genoaSpeed.text = "Genoa Force: " + genoaContribution;
        currentSpeed = currentSpeed * WindManager.instance.windMagnitude;
        Vector3 forceDir =
            transform.forward * (currentSpeed * speedFactor);
        _rigidbody.AddForce(
            forceDir, ForceMode.Force
        );
        // if (torqueEnabled)
        // {
        //     float force = mainSail.TorqueForce() + frontSail.TorqueForce();
        //     //Debug.Log("Torque: " + force);
        //     _rigidbody.AddRelativeTorque(0, 0,
        //         -force * torqueMultiplier
        //         , ForceMode.Force);
        // }
        speedText.text = "Speed: " + (int) (_rigidbody.velocity.magnitude * 100);

        //gameObject.transform.Rotate(0, dir.x * turningFactor, 0);
        //_rigidbody.AddForceAtPosition(transform.right * (dir.x * turningFactor) / 100f, tillerPos.position);
        if (dir.x > 0 && (tillerPos.localRotation.eulerAngles.y < 80 || tillerPos.localRotation.eulerAngles.y > 275))
        {
            tillerPos.RotateAround(tillerOrigin.position, Vector3.up, dir.x*tillerSensitivity);
        }

        if (dir.x < 0 && (tillerPos.localRotation.eulerAngles.y > 280 || tillerPos.localRotation.eulerAngles.y < 85))
        {
            tillerPos.RotateAround(tillerOrigin.position, Vector3.up, dir.x*tillerSensitivity);
        }
        
        currentTillerPos = tillerPos.localRotation.y;
        _rigidbody.AddForceAtPosition(transform.right * (currentTillerPos * turningFactor * Mathf.Clamp(_rigidbody.velocity.magnitude,1,100)), tillerPos.position);

        if (rightGenoaGrabbed)
        {
            if (genoa.rope >= 0.002 && dirRope.y < 0)
            {
                genoa.rope += dirRope.y / 1000;
                if (!ropeTight.isPlaying)
                {
                    ropeTight.Play();
                }

                if (ropeUnwind.isPlaying)
                {
                    ropeUnwind.Stop();
                }
            }

            if (genoa.rope < 0.20 && dirRope.y > 0)
            {
                genoa.rope += dirRope.y / 1000;
                if (ropeTight.isPlaying)
                {
                    ropeTight.Stop();
                }

                if (!ropeUnwind.isPlaying)
                {
                    ropeUnwind.Play();
                }
            }

            if (genoa.rope < 0.002) genoa.rope = 0.002f;
        }

        leftGenoaRope.text = "Front Sail Rope: " + (int) (genoa.rope * 100);

        if (mainSailGrabbed)
        {
            if (mainsail.rope >= 0.002 && dirRope.y < 0)
            {
                mainsail.rope += dirRope.y / 1000;
                if (!ropeTight.isPlaying)
                {
                    ropeTight.Play();
                }

                if (ropeUnwind.isPlaying)
                {
                    ropeUnwind.Stop();
                }
            }

            if (mainsail.rope < 0.20 && dirRope.y > 0)
            {
                mainsail.rope += dirRope.y / 1000;
                if (ropeTight.isPlaying)
                {
                    ropeTight.Stop();
                }

                if (!ropeUnwind.isPlaying)
                {
                    ropeUnwind.Play();
                }
            }

            if (mainsail.rope < 0.002) mainsail.rope = 0.002f;
        }

        mainSailRope.text = "Main Sail Rope: " + (int) (mainsail.rope * 100);

        if (!mainSailGrabbed && !rightGenoaGrabbed)
        {
            ropeTight.Stop();
            ropeUnwind.Stop();
        }
    }

    public void GrabLeft(InputAction.CallbackContext cx)
    {
        leftGenoaGrabbed = cx.ReadValueAsButton();
    }

    public void GrabRight(InputAction.CallbackContext cx)
    {
        rightGenoaGrabbed = cx.ReadValueAsButton();
    }

    public void MainSail(InputAction.CallbackContext cx)
    {
        mainSailGrabbed = cx.ReadValueAsButton();
    }

    public void ReleaseManual(InputAction.CallbackContext cx)
    {
        if (leftGenoaGrabbed)
        {
            //Release / Catch Left Genoa
            if (leftGenoaLineAttached)
            {
                genoa.rope = 1;
                leftGenoaRope.text = " Left Genoa Detached";
            }
            else
            {
                genoa.rope = 0.1f;
                leftGenoaRope.text = "Left Genoa: " + genoa.rope;
            }

            leftGenoaLineAttached = !leftGenoaLineAttached;
        }
    }

    public void Reload(InputAction.CallbackContext cx)
    {
        SceneManager.LoadScene("BoatMechanics");
    }

    public void HandleRopeManual(InputAction.CallbackContext cx)
    {
        dirRope = (Vector2) cx.ReadValueAsObject();
    }

    public void Tiller(InputAction.CallbackContext cx)
    {
        dir = (Vector2) cx.ReadValueAsObject();
    }
}