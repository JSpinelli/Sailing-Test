using UnityAtoms.BaseAtoms;
using UnityEngine;

public class FrontSailBehaviour : MonoBehaviour
{
    public GameObject mast;
    public float adjustmentFactor = 2f;
    public float windAttachmentFactor = 1.5f;
    
    public FloatReference rope;
    public StringReference pointOfSail;
    public Transform shipForward;
    
    public Vector2Reference runningRange;
    public Vector2Reference closeHauledRange;
    public Vector2Reference closeReachRange;
    public Vector2Reference beamReachRange;
    public Vector2Reference broadReachRange;
    public FloatReference frontSailContribution;
    
    public AnimationCurve sailForceCurve;
    public BoolReference frontSailWorking;
    
    private float curvePoint;

    void Update()
    {
        float dotRight = Vector2.Dot(transform.right, WindManager.instance.wind.normalized);
        float dotForward = Vector2.Dot(transform.forward, WindManager.instance.wind.normalized);
        float angle = Vector3.Angle(transform.forward, shipForward.forward);

        if (angle > rope.Value)
        {
            transform.RotateAround(mast.transform.position, transform.up,
                (Mathf.Sign(-transform.localRotation.y) * adjustmentFactor)/ WindManager.instance.windMagnitude);
        }
        else
        {
            transform.RotateAround(mast.transform.position, transform.up,
                -Mathf.Sign(dotRight) * WindManager.instance.windMagnitude * (1 - dotForward) *
                windAttachmentFactor * (Mathf.Abs((rope.Value-angle) / rope.Value)));
        }
        
        UpdateContribution();
    }
    
    private float SailForce()
    {
        Vector2 sailDirection = new Vector2(gameObject.transform.forward.x, gameObject.transform.forward.z);
        float dot = Vector2.Dot(sailDirection.normalized, WindManager.instance.wind.normalized);
        return Mathf.Abs(dot);
    }

    void UpdateContribution()
    {
        Vector2 frontSailSpread = Vector2.zero;
        switch (pointOfSail.Value)
        {
            case "In Irons":
            {
                frontSailContribution.Value = Mathf.Lerp(frontSailContribution.Value, 0, Time.deltaTime);
                if (GameManager.Instance.autoSailPositioning) rope.Value = 5;
                break;
            }
            case "Close Hauled":
            {
                frontSailSpread = closeHauledRange.Value;
                if (GameManager.Instance.autoSailPositioning) rope.Value = 10;
                break;
            }
            case "Close Reach":
            {
                frontSailSpread = closeReachRange.Value;
                if (GameManager.Instance.autoSailPositioning) rope.Value = 20;
                break;
            }
            case "Beam Reach":
            {
                frontSailSpread = beamReachRange.Value;
                if (GameManager.Instance.autoSailPositioning) rope.Value = 35;
                break;
            }
            case "Broad Reach":
            {
                frontSailSpread = broadReachRange.Value;
                if (GameManager.Instance.autoSailPositioning) rope.Value = 50;
                break;
            }
            case "Running":
            {
                frontSailSpread = runningRange.Value;
                if (GameManager.Instance.autoSailPositioning) rope.Value = 60;
                break;
            }
        }

        float force = SailForce();
        if (frontSailSpread.x <= force && force <= frontSailSpread.y)
        {
            curvePoint = (force - frontSailSpread.x) /
                         (frontSailSpread.y - frontSailSpread.x);
            frontSailContribution.Value = sailForceCurve.Evaluate(curvePoint);
            frontSailWorking.Value = true;
        }
        else
        {
            frontSailWorking.Value = false;
        }
    }
}
