using System;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatManager : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Sail mainSail;
    public Sail frontSail;
    public float speedFactor = 50f;

    public float torqueModifier;

    public AudioSource ropeTight;
    public AudioSource ropeUnwind;

    public float turningFactor = 0.5f;

    private float _currentSpeed = 0;

    public bool mainSailWorking = false;
    public bool frontSailWorking = false;

    public Transform tillerPos;
    public Transform tillerOrigin;

    private float m_currentTillerPos = 0;
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

    public FloatReference mainSailContribution;
    public FloatReference frontSailContribution;

    public bool autoSail = false;
    [Range(0f, 1f)] public float mainSailContributionAuto;
    [Range(0f, 1f)] public float frontSailContributionAuto;

    public float ropeStep = .5f;

    [HideInInspector] public float dot2;


    private bool triggeredSplash = false;
    private bool splashHappening = false;
    private float splashtimer = 0;
    public float splashDuration = 2;
    private float previousYVel;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        // HERE FOR BALANCING PURPOSES, THIS GET CHANGE AUTOMATICALLY WHEN ADDING A COLLIDER
        _rigidbody.inertiaTensor = new Vector3(1, 1, 1);
        previousYVel = _rigidbody.velocity.y;
    }

    // private void Update()
    // {
    //     if (previousYVel < _rigidbody.velocity.y && !triggeredSplash)
    //     {
    //         Gamepad.current.SetMotorSpeeds(0.5f, 0);
    //         splashHappening = true;
    //         triggeredSplash = true;
    //     }
    //     if (previousYVel > _rigidbody.velocity.y && !triggeredSplash)
    //     {
    //         previousYVel = _rigidbody.velocity.y;
    //     }
    //     if (previousYVel > _rigidbody.velocity.y && triggeredSplash)
    //     {
    //         triggeredSplash = false;
    //     }
    //     if (previousYVel < _rigidbody.velocity.y && triggeredSplash)
    //     {
    //         previousYVel = _rigidbody.velocity.y;
    //     }
    //     Splash();
    // }

    private void Splash()
    {
        if (splashHappening)
        {
            if (splashtimer < splashDuration)
            {
                splashtimer += Time.deltaTime;
            }
            else
            {
                splashtimer = 0;
                splashHappening = false;
                Gamepad.current.SetMotorSpeeds(0, 0);
            }
        }
    }

    private void FixedUpdate()
    {
        float frontSailForce = frontSail.SailForce();
        
        Vector2 sailDirection = new Vector2(transform.forward.x, transform.forward.z);
        Vector2 sailDirection2 = new Vector2(transform.right.x, transform.right.z);
        float dot = Vector2.Dot(sailDirection.normalized, WindManager.instance.wind.normalized);

        //Direction of rotation of the hull
        dot2 = Vector2.Dot(sailDirection2.normalized, WindManager.instance.wind.normalized);
        // Force of the rotation based on the position of the sails
        float dot3 = Vector3.Dot(gameObject.transform.up, Vector3.right);

        _currentSpeed = 0;
        if (!autoSail)
        {
            float mainSailMin = 0;
            float mainSailMax = 0;
            Vector2 frontSailSpread = Vector2.zero;
            switch (dot)
            {
                case { } f when (f <= WindManager.instance.noGo):
                    typeOfSailing.Value = "In Irons";
                    mainSailContribution.Value = Mathf.Lerp(mainSailContribution.Value, 0, Time.deltaTime);
                    frontSailContribution.Value = Mathf.Lerp(frontSailContribution.Value, 0, Time.deltaTime);
                    break;
                case { } f when (f > WindManager.instance.noGo && f <= -0.7):
                    // CLOSE HAUL
                    typeOfSailing.Value = "Close Hauled";
                    mainSailMax = 15;
                    mainSailMin = 0;
                    frontSailSpread = closeHauledRange.Value;
                    break;
                case { } f when (f > -0.7 && f <= -0.1):
                    // CLOSE REACH
                    typeOfSailing.Value = "Close Reach";
                    mainSailMax = 25;
                    mainSailMin = 10;
                    frontSailSpread = closeReachRange.Value;
                    break;
                case { } f when (f > -0.1 && f <= 0.1):
                    // BEAM REACH
                    typeOfSailing.Value = "Beam Reach";
                    mainSailMax = 35;
                    mainSailMin = 20;
                    frontSailSpread = beamReachRange.Value;
                    break;
                case { } f when (f > 0.1 && f <= 0.9):
                    //BROAD REACH
                    typeOfSailing.Value = "Broad Reach";
                    mainSailMax = 45;
                    mainSailMin = 30;
                    frontSailSpread = broadReachRange.Value;
                    break;
                case { } f when (f > 0.9):
                    //RUNNING
                    typeOfSailing.Value = "Running";
                    mainSailMax = 55;
                    mainSailMin = 40;
                    frontSailSpread = runningRange.Value;
                    break;
            }

            if (mainSailRope.Value < mainSailMax && mainSailRope > mainSailMin)
            {
                mainSailContribution.Value = Mathf.Lerp(mainSailContribution.Value, 1, Time.deltaTime);
                _currentSpeed += mainSailContribution;
                mainSailWorking = true;
            }
            else
            {
                mainSailContribution.Value = Mathf.Lerp(mainSailContribution.Value, .5f, Time.deltaTime);
                _currentSpeed += mainSailContribution;
                mainSailWorking = false;
            }

            if (frontSailSpread.x <= frontSailForce && frontSailForce <= frontSailSpread.y)
            {
                curvePoint = (frontSailForce - frontSailSpread.x) /
                             (frontSailSpread.y - frontSailSpread.x);
                frontSailContribution.Value = sailForceCurve.Evaluate(curvePoint);
                _currentSpeed += frontSailContribution.Value;
                frontSailWorking = true;
            }
            else
            {
                frontSailWorking = false;
            }
           
        }
        else
        {
            mainSailContribution.Value = mainSailContributionAuto;
            frontSailContribution.Value = frontSailContributionAuto;
            _currentSpeed += mainSailContribution + frontSailContribution.Value;
        }

        _currentSpeed = _currentSpeed * WindManager.instance.windMagnitude;
        Vector3 forceDir =
            transform.forward * (_currentSpeed * speedFactor);
        _rigidbody.AddForce(
            forceDir, ForceMode.Force
        );

        _rigidbody.AddTorque(new Vector3(0, 0, -dot2).normalized *
                             (torqueModifier * WindManager.instance.windMagnitude * _rigidbody.mass *
                              (1 - Mathf.Abs(dot3))));

        speed.Value = (int) (_rigidbody.velocity.magnitude * 100);

        TillerUpdate();
        SailUpdateDegrees();
    }

    private void TillerUpdate()
    {
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
        _rigidbody.AddForceAtPosition(
            transform.right * (tillerVal * turningFactor * Mathf.Clamp(_rigidbody.velocity.magnitude, 1, 50)),
            tillerPos.position);
    }

    private void SailUpdateDegrees()
    {
        if (PlayerController.leftTrigger)
        {
            //m_frontSailMat.SetColor("_Tint", grabColor);
            if (frontSailRope.Value >= 2)
            {
                frontSailRope.Value -= ropeStep;
                if (!ropeTight.isPlaying)
                {
                    ropeTight.Play();
                }

                if (ropeUnwind.isPlaying)
                {
                    ropeUnwind.Stop();
                }
            }

            if (frontSailRope.Value < 0.2) frontSailRope.Value = 2f;
        }


        if (PlayerController.rightTrigger)
        {
            //m_frontSailMat.SetColor("_Tint", grabColor);
            if (frontSailRope.Value < 80)
            {
                frontSailRope.Value += ropeStep;
                if (ropeTight.isPlaying)
                {
                    ropeTight.Stop();
                }

                if (!ropeUnwind.isPlaying)
                {
                    ropeUnwind.Play();
                }
            }
        }

        if (!PlayerController.rightTrigger && !PlayerController.leftTrigger)
        {
            //m_frontSailMat.SetColor("_Tint", m_originalColor);
            ropeTight.Stop();
            ropeUnwind.Stop();
        }

        if (PlayerController.rightBumper)
        {
            PlayerController.rightBumper = false;
            //m_mainSailMat.SetColor("_Tint", grabColor);
            if (mainSailRope.Value < 52)
            {
                mainSailRope.Value += 10;
                if (ropeTight.isPlaying)
                {
                    ropeTight.Stop();
                }

                if (!ropeUnwind.isPlaying)
                {
                    ropeUnwind.Play();
                }
            }
        }

        if (PlayerController.leftBumper)
        {
            PlayerController.leftBumper = false;
            //m_mainSailMat.SetColor("_Tint", grabColor);
            if (mainSailRope.Value >= 2)
            {
                mainSailRope.Value -= 10;
                if (!ropeTight.isPlaying)
                {
                    ropeTight.Play();
                }

                if (ropeUnwind.isPlaying)
                {
                    ropeUnwind.Stop();
                }
            }
        }

        if (!PlayerController.rightBumper && !PlayerController.leftBumper)
        {
            //m_mainSailMat.SetColor("_Tint", m_originalColor);
            ropeTight.Stop();
            ropeUnwind.Stop();
        }
    }
}