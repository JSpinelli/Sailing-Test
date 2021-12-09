using UnityAtoms.BaseAtoms;
using UnityEngine;

public class FrontSailBehaviour : MonoBehaviour
{
    public FloatReference rope;
    public StringReference pointOfSail;

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
                if (GameManager.Instance.autoFrontSailPositioning) rope.Value = 5;
                break;
            }
            case "Close Hauled":
            {
                frontSailSpread = closeHauledRange.Value;
                if (GameManager.Instance.autoFrontSailPositioning) rope.Value = 10;
                break;
            }
            case "Close Reach":
            {
                frontSailSpread = closeReachRange.Value;
                if (GameManager.Instance.autoFrontSailPositioning) rope.Value = 25;
                break;
            }
            case "Beam Reach":
            {
                frontSailSpread = beamReachRange.Value;
                if (GameManager.Instance.autoFrontSailPositioning) rope.Value = 40;
                break;
            }
            case "Broad Reach":
            {
                frontSailSpread = broadReachRange.Value;
                if (GameManager.Instance.autoFrontSailPositioning) rope.Value = 55;
                break;
            }
            case "Running":
            {
                frontSailSpread = runningRange.Value;
                if (GameManager.Instance.autoFrontSailPositioning) rope.Value = 60;
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
