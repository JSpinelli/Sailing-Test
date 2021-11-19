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
    private Rigidbody m_rigidbody;
    public Sail mainSail;
    public Sail frontSail;

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

    private float m_currentSpeed = 0;

    public ManualSailPhysics mainsail;
    public ManualSailPhysics genoa;

    private Vector2 m_dir;

    private Vector2 m_dirRope;

    public bool mainSailWorking = false;
    public bool frontSailWorking = false;

    public Transform tillerPos;
    public Transform tillerOrigin;

    private float m_currentTillerPos = 0;

    private Vector3 m_currentAngle;

    public float tillerSensitivity = 2f;

    public AnimationCurve tillerVelocity;

    public MeshRenderer tillerOutline;
    public MeshRenderer mainSailOutline;
    public MeshRenderer frontSailOutline;

    private Material m_tillerMat;
    private Material m_mainSailMat;
    private Material m_frontSailMat;

    public Color grabColor;
    private Color m_originalColor;
    private Color m_tilerOriginalColor;

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
                typeOfSailing.text = "In Irons";
                break;
            case float f when (f > WindManager.instance.noGo && f <= -0.7):
                // CLOSE HAUL
                typeOfSailing.text = "Close Hauled";
                if (0.75f <= mainSailForce && mainSailForce <= 0.95f)
                {
                    mainSailContribution = 1 - (Math.Abs(0.85f - mainSailForce) / 0.1f);
                    m_currentSpeed += mainSailContribution;
                    mainSailWorking = true;
                }
                else
                {
                    mainSailWorking = false;
                }

                if (0.75f <= frontSailForce && frontSailForce <= 0.95f)
                {
                    genoaContribution = 1 - (Math.Abs(0.85f - frontSailForce) / 0.1f);
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
                typeOfSailing.text = "Close Reach";
                if (0.75f <= mainSailForce && mainSailForce <= 0.95f)
                {
                    mainSailContribution = 1 - (Math.Abs(0.85f - mainSailForce) / 0.1f);
                    m_currentSpeed += mainSailContribution;
                    mainSailWorking = true;
                }
                else
                {
                    mainSailWorking = false;
                }

                if (0.75f <= frontSailForce && frontSailForce <= 0.95f)
                {
                    genoaContribution = 1 - (Math.Abs(0.85f - frontSailForce) / 0.1f);
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
                typeOfSailing.text = "Beam Reach";
                if (0.3f <= mainSailForce && mainSailForce <= 0.7f)
                {
                    mainSailContribution = 1 - (Math.Abs(0.5f - mainSailForce) / 0.2f);
                    m_currentSpeed += mainSailContribution;
                    mainSailWorking = true;
                }
                else
                {
                    mainSailWorking = false;
                }

                if (0.3f <= frontSailForce && frontSailForce <= 0.7f)
                {
                    genoaContribution = 1 - (Math.Abs(0.5f - frontSailForce) / 0.2f);
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
                typeOfSailing.text = "Broad Reach";
                if (0.01f <= mainSailForce && mainSailForce <= 0.5f)
                {
                    mainSailContribution = 1 - (Math.Abs(0.25f - mainSailForce) / 0.25f);
                    m_currentSpeed += mainSailContribution;
                    mainSailWorking = true;
                }
                else
                {
                    mainSailWorking = false;
                }

                if (0.01f <= frontSailForce && frontSailForce <= 0.5f)
                {
                    genoaContribution = 1 - (Math.Abs(0.25f - frontSailForce) / 0.25f);
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
                typeOfSailing.text = "Running";
                if (0.01f <= mainSailForce && mainSailForce <= 0.4f)
                {
                    mainSailContribution = 1 - (Math.Abs(0.2f - mainSailForce) / 0.2f);
                    m_currentSpeed += mainSailContribution;
                    mainSailWorking = true;
                }
                else
                {
                    mainSailWorking = false;
                }

                if (0.01f <= frontSailForce && frontSailForce <= 0.4f)
                {
                    genoaContribution = 1 - (Math.Abs(0.2f - frontSailForce) / 0.2f);
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
        mainSailSpeed.text = "Main Sail Force: " + mainSailContribution;
        genoaSpeed.text = "Genoa Force: " + genoaContribution;
        m_currentSpeed = m_currentSpeed * WindManager.instance.windMagnitude;
        Vector3 forceDir =
            transform.forward * (m_currentSpeed * speedFactor);
        m_rigidbody.AddForce(
            forceDir, ForceMode.Force
        );

        speedText.text = "Speed: " + (int) (m_rigidbody.velocity.magnitude * 100);

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
                PlayerController.tillerDir.x * tillerSensitivity);
        }

        if (PlayerController.tillerDir.x < 0 &&
            (tillerPos.localRotation.eulerAngles.y > 280 || tillerPos.localRotation.eulerAngles.y < 85))
        {
            tillerPos.RotateAround(tillerOrigin.position, tillerOrigin.up,
                PlayerController.tillerDir.x * tillerSensitivity);
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
            if (genoa.rope >= 2 && PlayerController.ropeDir.y < 0)
            {
                genoa.rope += PlayerController.ropeDir.y;
                if (!ropeTight.isPlaying)
                {
                    ropeTight.Play();
                }

                if (ropeUnwind.isPlaying)
                {
                    ropeUnwind.Stop();
                }
            }

            if (genoa.rope < 80 && PlayerController.ropeDir.y > 0)
            {
                genoa.rope += PlayerController.ropeDir.y;
                if (ropeTight.isPlaying)
                {
                    ropeTight.Stop();
                }

                if (!ropeUnwind.isPlaying)
                {
                    ropeUnwind.Play();
                }
            }

            if (genoa.rope < 0.2) genoa.rope = 2f;
        }
        else
        {
            m_frontSailMat.SetColor("_Tint",m_originalColor);
        }

        leftGenoaRope.text = "Front Sail Rope: " + (int) (genoa.rope);

        if (PlayerController.mainSailGrabbed)
        {
            m_mainSailMat.SetColor("_Tint",grabColor);
            if (mainsail.rope >= 2 && PlayerController.ropeDir.y < 0)
            {
                mainsail.rope += PlayerController.ropeDir.y;
                if (!ropeTight.isPlaying)
                {
                    ropeTight.Play();
                }

                if (ropeUnwind.isPlaying)
                {
                    ropeUnwind.Stop();
                }
            }

            if (mainsail.rope < 80 && PlayerController.ropeDir.y > 0)
            {
                mainsail.rope += PlayerController.ropeDir.y;
                if (ropeTight.isPlaying)
                {
                    ropeTight.Stop();
                }

                if (!ropeUnwind.isPlaying)
                {
                    ropeUnwind.Play();
                }
            }

            if (mainsail.rope < 2) mainsail.rope = 2;
        }        else
        {
            m_mainSailMat.SetColor("_Tint",m_originalColor);
        }


        mainSailRope.text = "Main Sail Rope: " + (int) (mainsail.rope);

        if (!PlayerController.rightGenoaGrabbed && !PlayerController.mainSailGrabbed)
        {
            ropeTight.Stop();
            ropeUnwind.Stop();
        }
    }
}