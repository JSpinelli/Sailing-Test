using System;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

public class ManualSailPhysics : MonoBehaviour
{
    public GameObject mast;
    public float adjustmentFactor = 2f;
    public float windAttachmentFactor = 1.5f;
    //[Range(0.0001f, 0.15f)] public float rope;
    public FloatReference rope;
    public TextMeshProUGUI text;
    public string mySail;
    public Transform shipForward;

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
                windAttachmentFactor * (Math.Abs((rope.Value-angle) / rope.Value)));
        }

        text.text = mySail + " Fwd: " + dotForward + " Right: " + dotRight;
    }
}