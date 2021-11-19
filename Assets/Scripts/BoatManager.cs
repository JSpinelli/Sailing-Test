﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BoatManager : MonoBehaviour
{
    private Rigidbody m_rigidbody;
    public Sail mainSail;
    public Sail frontSail;
    public float speedFactor = 50f;

    public AudioSource ropeTight;
    public AudioSource ropeUnwind;

    public float turningFactor = 0.5f;

    private float m_currentSpeed = 0;

    private Vector2 m_dir;

    private Vector2 m_dirRope;

    public bool mainSailWorking = false;
    public bool frontSailWorking = false;

    public Transform tillerPos;
    public Transform tillerOrigin;

    private float m_currentTillerPos = 0;

    private Vector3 m_currentAngle;

    public MeshRenderer tillerOutline;
    public MeshRenderer mainSailOutline;
    public MeshRenderer frontSailOutline;

    private Material m_tillerMat;
    private Material m_mainSailMat;
    private Material m_frontSailMat;

    public Color grabColor;
    private Color m_originalColor;
    private Color m_tilerOriginalColor;
    private float curvePoint;
    
    public AnimationCurve sailForceCurve;
    public AnimationCurve tillerVelocity;
    
    public FloatReference tillerSensitivity;
    public Vector2Reference runningRange;
    public Vector2Reference closeHauledRange;
    public Vector2Reference closeReachRange;
    public Vector2Reference beamReachRange;
    public Vector2Reference broadReachRange;
    public IntReference speed;
    public StringReference typeOfSailing;
    public FloatReference mainSailRope;
    public FloatReference frontSailRope;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.inertiaTensor = new Vector3(1, 1, 1);
        m_tillerMat = tillerOutline.material;
        m_mainSailMat = mainSailOutline.material;
        m_frontSailMat = frontSailOutline.material;
        m_mainSailMat.SetFloat("_Magnitude",0f);
        m_frontSailMat.SetFloat("_Magnitude",0f);
        m_originalColor = m_mainSailMat.GetColor("_Tint");
        m_tilerOriginalColor = m_tillerMat.GetColor("_Tint");
    }

    private void FixedUpdate()
    {
        float mainSailForce = mainSail.SailForce();
        float frontSailForce = frontSail.SailForce();
        Vector2 sailDirection = new Vector2(gameObject.transform.forward.x, gameObject.transform.forward.z);
        float dot = Vector2.Dot(sailDirection.normalized, WindManager.instance.wind.normalized);

        float mainSailContribution = 0;
        float genoaContribution = 0;
        m_currentSpeed = 0;
        switch (dot)
        {
            case float f when (f <= WindManager.instance.noGo):
                typeOfSailing.Value = "In Irons";
                break;
            case float f when (f > WindManager.instance.noGo && f <= -0.7):
                // CLOSE HAUL
                typeOfSailing.Value = "Close Hauled";
                if (closeHauledRange.Value.x <= mainSailForce && mainSailForce <= closeHauledRange.Value.y)
                {
                    curvePoint = (mainSailForce - closeHauledRange.Value.x) /
                                 (closeHauledRange.Value.y - closeHauledRange.Value.x);
                    mainSailContribution = sailForceCurve.Evaluate(curvePoint);
                    m_currentSpeed += mainSailContribution;
                    mainSailWorking = true;
                }
                else
                {
                    mainSailWorking = false;
                }

                if (closeHauledRange.Value.x <= frontSailForce && frontSailForce <= closeHauledRange.Value.y)
                {
                    curvePoint = (frontSailForce - closeHauledRange.Value.x) /
                                 (closeHauledRange.Value.y - closeHauledRange.Value.x);
                    genoaContribution = sailForceCurve.Evaluate(curvePoint);
                    m_currentSpeed += genoaContribution;
                    frontSailWorking = true;
                }
                else
                {
                    frontSailWorking = false;
                }

                break;
            case float f when (f > -0.7 && f <= -0.1):
                // CLOSE REACH
                typeOfSailing.Value = "Close Reach";
                if (closeReachRange.Value.x <= mainSailForce && mainSailForce <= closeReachRange.Value.y)
                {
                    curvePoint = (mainSailForce - closeReachRange.Value.x) /
                                 (closeReachRange.Value.y - closeReachRange.Value.x);
                    mainSailContribution = sailForceCurve.Evaluate(curvePoint);
                    m_currentSpeed += mainSailContribution;
                    mainSailWorking = true;
                }
                else
                {
                    mainSailWorking = false;
                }

                if (closeReachRange.Value.x <= frontSailForce && frontSailForce <= closeReachRange.Value.y)
                {
                    curvePoint = (frontSailForce - closeReachRange.Value.x) /
                                 (closeReachRange.Value.y - closeReachRange.Value.x);
                    genoaContribution = sailForceCurve.Evaluate(curvePoint);
                    m_currentSpeed += genoaContribution;
                    frontSailWorking = true;
                }
                else
                {
                    frontSailWorking = false;
                }

                break;
            case float f when (f > -0.1 && f <= 0.1):
                // BEAM REACH
                typeOfSailing.Value = "Beam Reach";
                if (beamReachRange.Value.x <= mainSailForce && mainSailForce <= beamReachRange.Value.y)
                {
                    curvePoint = (mainSailForce - beamReachRange.Value.x) /
                                 (beamReachRange.Value.y - beamReachRange.Value.x);
                    mainSailContribution = sailForceCurve.Evaluate(curvePoint);
                    m_currentSpeed += mainSailContribution;
                    mainSailWorking = true;
                }
                else
                {
                    mainSailWorking = false;
                }

                if (beamReachRange.Value.x <= frontSailForce && frontSailForce <= beamReachRange.Value.y)
                {
                    curvePoint = (frontSailForce - beamReachRange.Value.x) /
                                 (beamReachRange.Value.y - beamReachRange.Value.x);
                    genoaContribution = sailForceCurve.Evaluate(curvePoint);
                    m_currentSpeed += genoaContribution;
                    frontSailWorking = true;
                }
                else
                {
                    frontSailWorking = false;
                }

                break;
            case float f when (f > 0.1 && f <= 0.9):
                //BROAD REACH
                typeOfSailing.Value = "Broad Reach";
                if (broadReachRange.Value.x <= mainSailForce && mainSailForce <= broadReachRange.Value.y)
                {
                    curvePoint = (mainSailForce - broadReachRange.Value.x) /
                                 (broadReachRange.Value.y - broadReachRange.Value.x);
                    mainSailContribution = sailForceCurve.Evaluate(curvePoint);
                    m_currentSpeed += mainSailContribution;
                    mainSailWorking = true;
                }
                else
                {
                    mainSailWorking = false;
                }

                if (broadReachRange.Value.x <= frontSailForce && frontSailForce <= broadReachRange.Value.y)
                {
                    curvePoint = (frontSailForce - broadReachRange.Value.x) /
                                 (broadReachRange.Value.y - broadReachRange.Value.x);
                    genoaContribution = sailForceCurve.Evaluate(curvePoint);
                    m_currentSpeed += genoaContribution;
                    frontSailWorking = true;
                }
                else
                {
                    frontSailWorking = false;
                }

                break;
            case float f when (f > 0.9):
                //RUNNING
                typeOfSailing.Value = "Running";
                if (runningRange.Value.x <= mainSailForce && mainSailForce <= runningRange.Value.y)
                {
                    curvePoint = (mainSailForce - runningRange.Value.x) /
                                 (runningRange.Value.y - runningRange.Value.x);
                    mainSailContribution = sailForceCurve.Evaluate(curvePoint);
                    m_currentSpeed += mainSailContribution;
                    mainSailWorking = true;
                }
                else
                {
                    mainSailWorking = false;
                }

                if (runningRange.Value.x <= frontSailForce && frontSailForce <= runningRange.Value.y)
                {
                    curvePoint = (frontSailForce - runningRange.Value.x) /
                                 (runningRange.Value.y - runningRange.Value.x);
                    genoaContribution = sailForceCurve.Evaluate(curvePoint);
                    m_currentSpeed += genoaContribution;
                    frontSailWorking = true;
                }
                else
                {
                    frontSailWorking = false;
                }

                break;
        }
        m_mainSailMat.SetFloat("_Magnitude",(1- mainSailContribution) *0.13f);
        m_frontSailMat.SetFloat("_Magnitude",(1 - genoaContribution) *0.13f);
        m_currentSpeed = m_currentSpeed * WindManager.instance.windMagnitude;
        Vector3 forceDir =
            transform.forward * (m_currentSpeed * speedFactor);
        m_rigidbody.AddForce(
            forceDir, ForceMode.Force
        );

        speed.Value = (int) (m_rigidbody.velocity.magnitude * 100);

        TillerUpdate();
        SailUpdateDegrees();
    }

    private void TillerUpdate()
    {
        if (PlayerController.tillerGrabbed)
        {
            tillerOutline.material.SetColor("_Tint",grabColor);
        }
        else
        {
            tillerOutline.material.SetColor("_Tint",m_tilerOriginalColor);
        }

        if (PlayerController.tillerDir.x > 0 &&
            (tillerPos.localRotation.eulerAngles.y < 80 || tillerPos.localRotation.eulerAngles.y > 275))
        {
            tillerPos.RotateAround(tillerOrigin.position, tillerOrigin.up,
                PlayerController.tillerDir.x * tillerSensitivity.Value);
        }

        if (PlayerController.tillerDir.x < 0 &&
            (tillerPos.localRotation.eulerAngles.y > 280 || tillerPos.localRotation.eulerAngles.y < 85))
        {
            tillerPos.RotateAround(tillerOrigin.position, tillerOrigin.up,
                PlayerController.tillerDir.x * tillerSensitivity.Value);
        }

        m_currentTillerPos = tillerPos.localRotation.y;
        float tillerVal = Mathf.Sign(m_currentTillerPos) * tillerVelocity.Evaluate(Mathf.Abs(m_currentTillerPos));
        m_rigidbody.AddForceAtPosition(
            transform.right * (tillerVal * turningFactor * Mathf.Clamp(m_rigidbody.velocity.magnitude, 1, 50)),
            tillerPos.position);
    }

    private void SailUpdateDegrees()
    {
        if (PlayerController.rightGenoaGrabbed)
        {
            m_frontSailMat.SetColor("_Tint",grabColor);
            if (frontSailRope.Value >= 2 && PlayerController.ropeDir.y < 0)
            {
                frontSailRope.Value += PlayerController.ropeDir.y;
                if (!ropeTight.isPlaying)
                {
                    ropeTight.Play();
                }

                if (ropeUnwind.isPlaying)
                {
                    ropeUnwind.Stop();
                }
            }

            if (frontSailRope.Value < 80 && PlayerController.ropeDir.y > 0)
            {
                frontSailRope.Value += PlayerController.ropeDir.y;
                if (ropeTight.isPlaying)
                {
                    ropeTight.Stop();
                }

                if (!ropeUnwind.isPlaying)
                {
                    ropeUnwind.Play();
                }
            }

            if (frontSailRope.Value < 0.2) frontSailRope.Value = 2f;
        }
        else
        {
            m_frontSailMat.SetColor("_Tint",m_originalColor);
        }

        

        if (PlayerController.mainSailGrabbed)
        {
            m_mainSailMat.SetColor("_Tint",grabColor);
            if (mainSailRope.Value >= 2 && PlayerController.ropeDir.y < 0)
            {
                mainSailRope.Value  += PlayerController.ropeDir.y;
                if (!ropeTight.isPlaying)
                {
                    ropeTight.Play();
                }

                if (ropeUnwind.isPlaying)
                {
                    ropeUnwind.Stop();
                }
            }

            if (mainSailRope.Value  < 80 && PlayerController.ropeDir.y > 0)
            {
                mainSailRope.Value  += PlayerController.ropeDir.y;
                if (ropeTight.isPlaying)
                {
                    ropeTight.Stop();
                }

                if (!ropeUnwind.isPlaying)
                {
                    ropeUnwind.Play();
                }
            }

            if (mainSailRope.Value  < 2) mainSailRope.Value  = 2;
        }        else
        {
            m_mainSailMat.SetColor("_Tint",m_originalColor);
        }
        

        if (!PlayerController.rightGenoaGrabbed && !PlayerController.mainSailGrabbed)
        {
            ropeTight.Stop();
            ropeUnwind.Stop();
        }
    }
}