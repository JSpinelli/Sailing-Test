using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.InputSystem;

public class FrontSailBehaviour : MonoBehaviour
{
    public float defaultTorque = 1.2f;
    public float magnitudeMultiplier = 0.1f;
    public GameObject mast;
    public float adjustmentFactor = 2f;
    public float windAttachmentFactor = 1.5f;
    public FloatReference ropeLeft;
    public FloatReference ropeRight;
    public Transform shipForward;
    public float steps;
    public float ropeOffset = 10;
    public float SailForce()
    {
        Vector2 sailDirection = new Vector2(gameObject.transform.forward.x, gameObject.transform.forward.z);
        float dot = Vector2.Dot(sailDirection.normalized, WindManager.instance.wind.normalized);
        return Mathf.Abs(dot);
    }
    void Update()
    {
        float dotRight = Vector2.Dot(transform.right, WindManager.instance.wind.normalized);
        float dotForward = Vector2.Dot(transform.forward, WindManager.instance.wind.normalized);
        float angle = Vector3.Angle(transform.forward, shipForward.forward);
        float sideOfShipe = Vector3.Dot(transform.right, shipForward.forward);
        float rope = 0;
        if (sideOfShipe >= 0)
        {
            rope = ropeLeft.Value - ropeOffset > ropeRight.Value ? ropeRight.Value : ropeLeft.Value - ropeOffset;
        }
        else
        {
            rope = ropeRight.Value - ropeOffset > ropeLeft.Value ? ropeLeft.Value : ropeRight.Value - ropeOffset;
        }
        
        if (angle > rope)
        {
            transform.RotateAround(mast.transform.position, transform.up,
                (Mathf.Sign(-transform.localRotation.y) * adjustmentFactor)/ WindManager.instance.windMagnitude);
        }
        else
        {
            transform.RotateAround(mast.transform.position, transform.up,
                -Mathf.Sign(dotRight) * WindManager.instance.windMagnitude * (1 - dotForward) *
                windAttachmentFactor * (Mathf.Abs((rope-angle) / rope)));
        }
        //text.text = mySail + " Fwd: " + dotForward + " Right: " + dotRight;
    }
}
