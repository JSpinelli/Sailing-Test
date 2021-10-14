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

    public SpringJoint leftGenoaLine;
    public SpringJoint rightGenoaLine;
    public SpringJoint mainSailLine;

    public float speedFactor = 50f;

    public TextMeshProUGUI speedText;
    public TextMeshProUGUI mainSailSpeed;
    public TextMeshProUGUI genoaSpeed;
    public TextMeshProUGUI typeOfSailing;

    public TextMeshProUGUI mainSailRope;
    public TextMeshProUGUI leftGenoaRope;
    public TextMeshProUGUI rightGenoaRope;

    public float turningFactor = 0.5f;

    private float currentSpeed = 0;

    private bool leftGenoaLineAttached = true;
    private bool rightGenoaLineAttached = true;

    public Vector3 debuggingForward;

    public GameObject front;

    private bool leftGenoaGrabbed = false;
    private bool rightGenoaGrabbed = false;
    private bool mainSailGrabbed = false;


    public ManualSailPhysics mainsail;
    public ManualSailPhysics genoa;

    public Vector2 dir;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        controls = new PlayerControls();
    }

    private void Update()
    {
        debuggingForward =  front.transform.position - gameObject.transform.position;
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
                typeOfSailing.text = "NO GO";
                break;
            case float f when (f > WindManager.instance.noGo && f <= -0.7):
                // CLOSE HAUL
                typeOfSailing.text = "Close Haul";
                if (0.75f <= mainSailForce && mainSailForce <= 0.95f)
                {
                    mainSailContribution = 1 - (Math.Abs(0.85f - mainSailForce) / 0.1f);
                    currentSpeed += mainSailContribution;
                }

                if (0.75f <= frontSailForce && frontSailForce <= 0.95f)
                {
                    genoaContribution = 1 - (Math.Abs(0.85f - frontSailForce) / 0.1f);
                    currentSpeed += genoaContribution;
                }

                break;
            case float f when (f > -0.7 && f <= -0.1):
                // CLOSE REACH
                typeOfSailing.text = "Close Reach";
                if (0.75f <= mainSailForce && mainSailForce <= 0.95f)
                {
                    mainSailContribution = 1 - (Math.Abs(0.85f - mainSailForce) / 0.1f);
                    currentSpeed += mainSailContribution;
                }

                if (0.75f <= frontSailForce && frontSailForce <= 0.95f)
                {
                    genoaContribution = 1 - (Math.Abs(0.85f - frontSailForce) / 0.1f);
                    currentSpeed += genoaContribution;
                }

                break;
            case float f when (f > -0.1 && f <= 0.1):
                // BEAM REACH
                typeOfSailing.text = "Beam Reach";
                if (0.3f <= mainSailForce && mainSailForce <= 0.7f)
                {
                    mainSailContribution = 1 - (Math.Abs(0.5f - mainSailForce) / 0.2f);
                    currentSpeed += mainSailContribution;
                }

                if (0.3f <= frontSailForce && frontSailForce <= 0.7f)
                {
                    genoaContribution = 1 - (Math.Abs(0.5f - frontSailForce) / 0.2f);
                    currentSpeed += genoaContribution;
                }

                break;
            case float f when (f > 0.1 && f <= 0.9):
                //BROAD REACH
                typeOfSailing.text = "Broad Reach";
                if (0.01f <= mainSailForce && mainSailForce <= 0.5f)
                {
                    mainSailContribution = 1 - (Math.Abs(0.25f - mainSailForce) / 0.25f);
                    currentSpeed += mainSailContribution;
                }

                if (0.01f <= frontSailForce && frontSailForce <= 0.5f)
                {
                    genoaContribution = 1 - (Math.Abs(0.25f - frontSailForce) / 0.25f);
                    currentSpeed += genoaContribution;
                }

                break;
            case float f when (f > 0.9):
                //RUNNING
                typeOfSailing.text = "Running";
                if (0.01f <= mainSailForce && mainSailForce <= 0.4f)
                {
                    mainSailContribution = 1 - (Math.Abs(0.2f - mainSailForce) / 0.2f);
                    currentSpeed += mainSailContribution;
                }

                if (0.01f <= frontSailForce && frontSailForce <= 0.4f)
                {
                    genoaContribution = 1 - (Math.Abs(0.2f - frontSailForce) / 0.2f);
                    currentSpeed += genoaContribution;
                }

                break;
        }

        mainSailSpeed.text = "Main Sail Force: " + mainSailContribution;
        genoaSpeed.text = "Genoa Force: " + genoaContribution;
        currentSpeed = currentSpeed * WindManager.instance.windMagnitude;
        Vector3 forceDir =
            debuggingForward * (currentSpeed * speedFactor);
        // Debug.Log("Forward: "+ transform.forward);
        // Debug.Log("Debug forward: "+ debuggingForward);
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
        speedText.text = "Speed: " + _rigidbody.velocity.magnitude;
        
        gameObject.transform.Rotate(0,  dir.x * turningFactor, 0);
    }

    public void DefualtKeyBoardControls()
    {
         if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Rotate(0, -1 * _rigidbody.velocity.magnitude * turningFactor, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Rotate(0, 1 * _rigidbody.velocity.magnitude * turningFactor, 0);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Release / Catch Left Genoa
            if (leftGenoaLineAttached)
            {
                leftGenoaLine.minDistance = 80;
                leftGenoaRope.text = " Left Genoa Detached";
            }
            else
            {
                leftGenoaLine.minDistance = 1.5f;
                leftGenoaRope.text = "Left Genoa: " + leftGenoaLine.minDistance;
            }

            leftGenoaLineAttached = !leftGenoaLineAttached;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //Release / Catch Right Genoa
            if (rightGenoaLineAttached)
            {
                rightGenoaLine.minDistance = 80;
                rightGenoaRope.text = " Right Genoa Detached";
            }
            else
            {
                rightGenoaLine.minDistance = 1.5f;
                rightGenoaRope.text = "Right Genoa: " + rightGenoaLine.minDistance;
            }

            rightGenoaLineAttached = !rightGenoaLineAttached;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            if (mainSailLine.minDistance != 0)
            {
                mainSailLine.minDistance -= 0.01f;
            }

            mainSailRope.text = "Main Sail: " + mainSailLine.minDistance;
        }

        if (Input.GetKey(KeyCode.C))
        {
            if (mainSailLine.minDistance != 10f)
            {
                mainSailLine.minDistance += 0.01f;
            }

            mainSailRope.text = "Main Sail: " + mainSailLine.minDistance;
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            if (rightGenoaLineAttached)
            {
                if (rightGenoaLine.minDistance != 0)
                {
                    rightGenoaLine.minDistance -= 0.01f;
                }

                rightGenoaRope.text = "Right Genoa: " + rightGenoaLine.minDistance;
            }
        }

        if (Input.GetKey(KeyCode.Alpha4))
        {
            if (rightGenoaLineAttached)
            {
                if (rightGenoaLine.minDistance != 10f)
                {
                    rightGenoaLine.minDistance += 0.01f;
                }

                rightGenoaRope.text = "Right Genoa: " + rightGenoaLine.minDistance;
            }
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (leftGenoaLineAttached)
            {
                if (leftGenoaLine.minDistance != 0)
                {
                    leftGenoaLine.minDistance -= 0.01f;
                }

                leftGenoaRope.text = "Left Genoa: " + leftGenoaLine.minDistance;
            }
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            if (leftGenoaLineAttached)
            {
                if (leftGenoaLine.minDistance != 10f)
                {
                    leftGenoaLine.minDistance += 0.01f;
                }

                leftGenoaRope.text = "Left Genoa: " + leftGenoaLine.minDistance;
            }
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

    public void Release(InputAction.CallbackContext cx)
    {
        if (leftGenoaGrabbed)
        {
            //Release / Catch Left Genoa
            if (leftGenoaLineAttached)
            {
                leftGenoaLine.minDistance = 80;
                leftGenoaRope.text = " Left Genoa Detached";
            }
            else
            {
                leftGenoaLine.minDistance = 1.5f;
                leftGenoaRope.text = "Left Genoa: " + leftGenoaLine.minDistance;
            }

            leftGenoaLineAttached = !leftGenoaLineAttached;
        }

        if (rightGenoaGrabbed)
        {
            //Release / Catch Right Genoa
            if (rightGenoaLineAttached)
            {
                rightGenoaLine.minDistance = 80;
                rightGenoaRope.text = " Right Genoa Detached";
            }
            else
            {
                rightGenoaLine.minDistance = 1.5f;
                rightGenoaRope.text = "Right Genoa: " + rightGenoaLine.minDistance;
            }

            rightGenoaLineAttached = !rightGenoaLineAttached;
        }
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

    public void HandleRope(InputAction.CallbackContext cx)
    {
        Vector2 dir = cx.ReadValue<Vector2>();
        if (leftGenoaGrabbed)
        {
            if (leftGenoaLineAttached)
            {
                
                if (leftGenoaLine.minDistance >= 0 && dir.y < 0 )
                {
                    leftGenoaLine.minDistance += dir.y/10;
                }
                if (leftGenoaLine.minDistance < 5 && dir.y > 0 )
                    leftGenoaLine.minDistance += dir.y/10;
                leftGenoaRope.text = "Left Genoa: " + leftGenoaLine.minDistance;
            }
        }

        if (rightGenoaGrabbed)
        {
            if (rightGenoaLineAttached)
            {
                if (rightGenoaLine.minDistance >= 0 && dir.y < 0 )
                {
                    rightGenoaLine.minDistance += dir.y/10;
                }
                if (rightGenoaLine.minDistance < 5 && dir.y > 0 )
                    rightGenoaLine.minDistance += dir.y/10;
                rightGenoaRope.text = "Right Genoa: " + rightGenoaLine.minDistance;
            }
        }

        if (mainSailGrabbed)
        {
            if (mainSailLine.minDistance >= 0 && dir.y < 0 )
            {
                mainSailLine.minDistance += dir.y/10;
            }
            if (mainSailLine.minDistance < 5 && dir.y > 0 )
                mainSailLine.minDistance += dir.y/10;
            mainSailRope.text = "Main Sail: " + mainSailLine.minDistance;
        }
    }

    public void Reload(InputAction.CallbackContext cx)
    {
        SceneManager.LoadScene("BoatMechanics");
    }

    public void HandleRopeManual(InputAction.CallbackContext cx)
    {
        Vector2 dir = cx.ReadValue<Vector2>();
        if (leftGenoaGrabbed)
        {
            if (leftGenoaLineAttached)
            {
                
                if (genoa.rope >= 0 && dir.y < 0 )
                {
                    genoa.rope += dir.y/1000;
                }
                if (genoa.rope < 0.20 && dir.y > 0 )
                    genoa.rope += dir.y/1000;
                leftGenoaRope.text = "Left Genoa: " + genoa.rope;
            }
        }

        if (mainSailGrabbed)
        {
            if (mainsail.rope >= 0 && dir.y < 0 )
            {
                mainsail.rope += dir.y/1000;
            }
            if (mainsail.rope < 0.20 && dir.y > 0 )
                mainsail.rope += dir.y/1000;
            mainSailRope.text = "Main Sail: " + mainsail.rope;
        }
    }
    
    public void Tiller(InputAction.CallbackContext cx)
    {
        dir = (Vector2) cx.ReadValueAsObject();
    }
}