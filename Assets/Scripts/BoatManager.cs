using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoatManager : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Sail mainSail;
    public Sail frontSail;
    public bool torqueEnabled = true;
    public float torqueMultiplier = 10;

    public SpringJoint leftGenoaLine;
    public SpringJoint rightGenoaLine;
    public SpringJoint mainSailLine;

    public bool noSails = false;

    public TextMeshProUGUI speedText;
    public TextMeshProUGUI mainSailSpeed;
    public TextMeshProUGUI genoaSpeed;
    public TextMeshProUGUI typeOfSailing;

    public float turningFactor = 0.5f;

    private float currentSpeed = 0;

    private bool leftGenoaLineAttached = true;
    private bool rightGenoaLineAttached = true;

    public Vector3 debuggingForward;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        debuggingForward = gameObject.transform.forward;
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Rotate(0, -1 * _rigidbody.velocity.magnitude * turningFactor, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Rotate(0, 1 * _rigidbody.velocity.magnitude * turningFactor, 0);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            //Release / Catch Left Genoa
            if (leftGenoaLineAttached)
            {
                leftGenoaLine.minDistance = 80;
            }
            else
            {
                leftGenoaLine.minDistance = 1.5f;
            }

            leftGenoaLineAttached = !leftGenoaLineAttached;
        }

        if (Input.GetKey(KeyCode.E))
        {
            //Release / Catch Right Genoa
            if (rightGenoaLineAttached)
            {
                rightGenoaLine.minDistance = 80;
            }
            else
            {
                rightGenoaLine.minDistance = 1.5f;
            }

            rightGenoaLineAttached = !rightGenoaLineAttached;
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            if (rightGenoaLine.minDistance != 0)
            {
                rightGenoaLine.minDistance -= 0.01f;
            }
        }

        if (Input.GetKey(KeyCode.Alpha4))
        {
            if (rightGenoaLine.minDistance != 10f)
            {
                rightGenoaLine.minDistance += 0.01f;
            }
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (leftGenoaLine.minDistance != 0)
            {
                leftGenoaLine.minDistance -= 0.01f;
            }
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            if (leftGenoaLine.minDistance != 10f)
            {
                leftGenoaLine.minDistance += 0.01f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (noSails)
        {
            _rigidbody.AddForce(
                gameObject.transform.forward.normalized
            );
        }
        else
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
                    if (0.85f <= mainSailForce && mainSailForce <= 0.9f)
                    {
                        mainSailContribution = 1 - (Math.Abs(0.87f - mainSailForce) / 0.03f);
                        currentSpeed = mainSailContribution;
                    }

                    if (0.85f <= frontSailForce && frontSailForce <= 0.9f)
                    {
                        genoaContribution = 1 - (Math.Abs(0.87f - frontSailForce) / 0.03f);
                        currentSpeed += genoaContribution;
                    }

                    break;
                case float f when (f > -0.7 && f <= -0.1):
                    // CLOSE REACH
                    typeOfSailing.text = "Close Reach";
                    if (0.8f <= mainSailForce && mainSailForce <= 0.9f)
                    {
                        mainSailContribution = 1 - (Math.Abs(0.85f - mainSailForce) / 0.05f);
                        currentSpeed = mainSailContribution;
                    }

                    if (0.8f <= frontSailForce && frontSailForce <= 0.9f)
                    {
                        genoaContribution = 1 - (Math.Abs(0.85f - frontSailForce) / 0.05f);
                        currentSpeed += genoaContribution;
                    }

                    break;
                case float f when (f > -0.1 && f <= 0.1):
                    // BEAM REACH
                    typeOfSailing.text = "Beam Reach";
                    if (0.3f <= mainSailForce && mainSailForce <= 0.7f)
                    {
                        mainSailContribution = 1 - (Math.Abs(0.5f - mainSailForce) / 0.2f);
                        currentSpeed = mainSailContribution;
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
                        currentSpeed = mainSailContribution;
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
                    if (0.01f <= mainSailForce && mainSailForce <= 0.3f)
                    {
                        mainSailContribution = 1 - (Math.Abs(0.15f - mainSailForce) / 0.15f);
                        currentSpeed = mainSailContribution;
                    }

                    if (0.01f <= frontSailForce && frontSailForce <= 0.3f)
                    {
                        genoaContribution = 1 - (Math.Abs(0.15f - frontSailForce) / 0.15f);
                        currentSpeed += genoaContribution;
                    }

                    break;
            }

            mainSailSpeed.text = "Main Sail Force: " + mainSailContribution;
            genoaSpeed.text = "Genoa Force: " + genoaContribution;
            Debug.Log(currentSpeed);
            currentSpeed = currentSpeed * WindManager.instance.windMagnitude;
            Vector3 forceDir =
                gameObject.transform.forward.normalized * (currentSpeed * 5f);
            _rigidbody.AddForce(
                forceDir, ForceMode.Force
            );
            if (torqueEnabled)
            {
                float force = mainSail.TorqueForce() + frontSail.TorqueForce();
                Debug.Log("Torque: " + force);
                _rigidbody.AddRelativeTorque(0, 0,
                    -force * torqueMultiplier
                    , ForceMode.Force);
            }
        }

        speedText.text = "Speed: " + _rigidbody.velocity.magnitude;
    }
}