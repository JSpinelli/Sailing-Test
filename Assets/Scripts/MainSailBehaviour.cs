using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class MainSailBehaviour : MonoBehaviour
{
    public GameObject mast;
    public float adjustmentFactor = 2f;
    public float windAttachmentFactor = 1.5f;
    //[Range(0.0001f, 0.15f)] public float rope;
    
    public FloatReference rope;
    public FloatReference mainSailContribution;
    public StringReference pointOfSail;
    
    public Transform shipForward;
    
    public BoolReference mainSailWorking;
    
    
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
        float mainSailMin = 0;
        float mainSailMax = 0;
        switch (pointOfSail.Value)
        {
            case "In Irons":
            {
                mainSailContribution.Value = Mathf.Lerp(mainSailContribution.Value, 0, Time.deltaTime);
                if (GameManager.Instance.autoSailPositioning) rope.Value = 5;
                break;
            }
            case "Close Hauled":
            {
                mainSailMax = 15;
                mainSailMin = 0;
                if (GameManager.Instance.autoSailPositioning) rope.Value = 10;
                break;
            }
            case "Close Reach":
            {
                mainSailMax = 25;
                mainSailMin = 10;
                if (GameManager.Instance.autoSailPositioning) rope.Value = 20;
                break;
            }
            case "Beam Reach":
            {
                mainSailMax = 35;
                mainSailMin = 20;
                if (GameManager.Instance.autoSailPositioning) rope.Value = 30;
                break;
            }
            case "Broad Reach":
            {
                mainSailMax = 45;
                mainSailMin = 30;
                if (GameManager.Instance.autoSailPositioning) rope.Value = 40;
                break;
            }
            case "Running":
            {
                mainSailMax = 55;
                mainSailMin = 40;
                if (GameManager.Instance.autoSailPositioning) rope.Value = 50;
                break;
            }
        }
        if (rope.Value < mainSailMax && rope > mainSailMin)
        {
            mainSailContribution.Value = Mathf.Lerp(mainSailContribution.Value, 1, Time.deltaTime);
            mainSailWorking.Value = true;
        }
        else
        {
            mainSailContribution.Value = Mathf.Lerp(mainSailContribution.Value, .5f, Time.deltaTime);
            mainSailWorking.Value = false;
        }
    }
}
