using UnityAtoms.BaseAtoms;
using UnityEngine;

public class BoatManager : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _currentSpeed = 0;
    private float _currentTillerPos = 0;

    public float speedFactor = 50f;
    public float torqueModifier;
    public float turningFactor = 0.5f;
    public float ropeStep = .5f;
    public AnimationCurve tillerVelocity;

    public bool autoSail = false;
    public bool torqueEnabled = false;
    [Range(0f, 1f)] public float mainSailContributionAuto;
    [Range(0f, 1f)] public float frontSailContributionAuto;

    public AudioSource ropeTight;
    public AudioSource ropeUnwind;
    public Transform tillerPos;
    public Transform tillerOrigin;

    public FloatReference tillerSensitivity;
    public IntReference speed;
    public StringReference typeOfSailing;
    public FloatReference mainSailRope;
    public FloatReference frontSailRope;
    public FloatReference mainSailContribution;
    public FloatReference frontSailContribution;

    [HideInInspector] public float dot2;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        // HERE FOR BALANCING PURPOSES, THIS GET CHANGE AUTOMATICALLY WHEN ADDING A COLLIDER
        _rigidbody.inertiaTensor = new Vector3(1, 1, 1);
    }

    private void FixedUpdate()
    {
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

        Vector3 forceDir =
            transform.forward * (_currentSpeed * speedFactor);
        _rigidbody.AddForce(
            forceDir, ForceMode.Force
        );

        if (torqueEnabled)
        {
            _rigidbody.AddTorque(new Vector3(0, 0, -dot2).normalized *
                                 (torqueModifier * WindManager.instance.windMagnitude * _rigidbody.mass *
                                  (1 - Mathf.Abs(dot3))));
        }

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

        _currentTillerPos = tillerPos.localRotation.y;
        float tillerVal = Mathf.Sign(_currentTillerPos) * tillerVelocity.Evaluate(Mathf.Abs(_currentTillerPos));
        _rigidbody.AddForceAtPosition(
            transform.right * (tillerVal * turningFactor * Mathf.Clamp(_rigidbody.velocity.magnitude, 10, 50)),
            tillerPos.position);
    }

    private void SailUpdateDegrees()
    {
        if (PlayerController.leftTrigger)
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


        if (PlayerController.rightTrigger)
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

        if (!PlayerController.rightTrigger && !PlayerController.leftTrigger)
        {
            ropeTight.Stop();
            ropeUnwind.Stop();
        }

        if (PlayerController.rightBumper)
        {
            PlayerController.rightBumper = false;
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

        if (!PlayerController.rightBumper && !PlayerController.leftBumper)
        {
            ropeTight.Stop();
            ropeUnwind.Stop();
        }
    }
}