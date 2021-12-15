using UnityAtoms.BaseAtoms;
using UnityEngine;

public class BoatManager : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _currentSpeed;
    
    //Optimization variables
    private Vector3 _myForward;
    private Vector3 _myRight;
    private float _velMagnitude;

    public float speedFactor = 50f;
    public float torqueModifier;
    public float ropeStep = .5f;

    public bool autoSail;
    public bool torqueEnabled;
    [Range(0f, 1f)] public float mainSailContributionAuto;
    [Range(0f, 1f)] public float frontSailContributionAuto;

    public AudioSource ropeTight;
    public AudioSource ropeUnwind;
    public Transform tillerPos;
    
    public IntReference speed;
    public StringReference typeOfSailing;
    public FloatReference mainSailRope;
    public FloatReference frontSailRope;
    public FloatReference mainSailContribution;
    public FloatReference frontSailContribution;
    public FloatReference frontSailAngle;
    public FloatReference mainSailAngle;
    public FloatReference currentTillerPos;

    [HideInInspector] public float dot2;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        // HERE FOR BALANCING PURPOSES, THIS GET CHANGE AUTOMATICALLY WHEN ADDING A COLLIDER
        _rigidbody.inertiaTensor = new Vector3(1, 1, 1);
    }

    private void FixedUpdate()
    {
        _myForward = transform.forward;
        _myRight = transform.right;
        Vector2 sailDirection = new Vector2(_myForward.x, _myForward.z);
        Vector2 sailDirection2 = new Vector2(_myRight.x, _myRight.z);
        float dot = Vector2.Dot(sailDirection.normalized, WindManager.instance.wind.normalized);
        //Direction of rotation of the hull
        dot2 = Vector2.Dot(sailDirection2.normalized, WindManager.instance.wind.normalized);
        // Force of the rotation based on the position of the sails
        float dot3 = Vector3.Dot(gameObject.transform.up, Vector3.right);
        _currentSpeed = 0;
        if (!autoSail)
        {
            switch (dot)
            {
                case { } f when (f <= WindManager.instance.noGo):
                    typeOfSailing.Value = "In Irons";
                    break;
                case { } f when (f > WindManager.instance.noGo && f <= -0.7):
                    // CLOSE HAUL
                    typeOfSailing.Value = "Close Hauled";
                    break;
                case { } f when (f > -0.7 && f <= -0.1):
                    // CLOSE REACH
                    typeOfSailing.Value = "Close Reach";
                    break;
                case { } f when (f > -0.1 && f <= 0.1):
                    // BEAM REACH
                    typeOfSailing.Value = "Beam Reach";
                    break;
                case { } f when (f > 0.1 && f <= 0.9):
                    //BROAD REACH
                    typeOfSailing.Value = "Broad Reach";
                    break;
                case { } f when (f > 0.9):
                    //RUNNING
                    typeOfSailing.Value = "Running";
                    break;
            }
        }
        else
        {
            mainSailContribution.Value = mainSailContributionAuto;
            frontSailContribution.Value = frontSailContributionAuto;
        }

        _currentSpeed = mainSailContribution.Value + frontSailContribution.Value;
        _currentSpeed = _currentSpeed * WindManager.instance.windMagnitude;

        Vector3 forceDir = transform.forward * (_currentSpeed * speedFactor);
        
        _rigidbody.AddForce(forceDir, ForceMode.Force);

        if (torqueEnabled)
        {
            _rigidbody.AddTorque(new Vector3(0, 0, -dot2).normalized *
                                 (torqueModifier * WindManager.instance.windMagnitude * _rigidbody.mass *
                                  (1 - Mathf.Abs(dot3))));
        }

        _velMagnitude = _rigidbody.velocity.magnitude;
        speed.Value = (int) (_velMagnitude * 100);

        //Boat Turning
        _rigidbody.AddForceAtPosition(
            transform.right * (currentTillerPos * Mathf.Clamp(_velMagnitude, 10, 50)),
            tillerPos.position);
        
        SailUpdateDegrees();
    }

    private void SailUpdateDegrees()
    {
        if (PlayerController.leftTrigger)
        {
            if (frontSailAngle.Value < 0)
                TakeRopeFrontSail();
            else
                GiveRopeFrontSail();
        }

        if (PlayerController.rightTrigger)
        {
            if (frontSailAngle.Value > 0)
                TakeRopeFrontSail();
            else
                GiveRopeFrontSail();
        }

        if (!PlayerController.rightTrigger && !PlayerController.leftTrigger)
        {
            ropeTight.Stop();
            ropeUnwind.Stop();
        }

        if (PlayerController.rightBumper)
        {
            PlayerController.rightBumper = false;
            if (mainSailAngle.Value > 0)
                TakeRopeMainSail();
            else
                GiveRopeMainSail();
        }

        if (PlayerController.leftBumper)
        {
            PlayerController.leftBumper = false;
            if (mainSailAngle.Value < 0)
                TakeRopeMainSail();
            else
                GiveRopeMainSail();
        }

        if (!PlayerController.rightBumper && !PlayerController.leftBumper)
        {
            ropeTight.Stop();
            ropeUnwind.Stop();
        }
    }

    private void GiveRopeFrontSail()
    {
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

    private void TakeRopeFrontSail()
    {
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

    private void GiveRopeMainSail()
    {
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

    private void TakeRopeMainSail()
    {
        if (mainSailRope.Value >= 2)
        {
            mainSailRope.Value -= 10;
            if (mainSailRope.Value == 0)
            {
                mainSailRope.Value = 2;
            }

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
}
