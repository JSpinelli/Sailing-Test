using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class ManualSailPhysics : MonoBehaviour
{
    // Update is called once per frame
    public GameObject mast;

    //public Transform forward1;
    //public Transform forward2;
    //public Transform right1;
    //public Transform right2;

    public float adjustmentFactor = 2f;
    public float windAttachmentFactor = 1.5f;

    [Range(0.0001f, 0.15f)] public float rope;
    public MeshRenderer sail;
    public Material defaultMat;
    public Material correctingMat;

    public TextMeshProUGUI text;
    public string mySail;

    public bool showRotText = false;
    public TextMeshProUGUI rotText;

    public bool useForward = false;

    void Update()
    {
        //Vector3 right = right2.position - right1.position;
        //Vector3 forward = forward2.position - forward1.position;
        // Vector2 sailDirectionRight = new Vector2(right.x, right.z);
        // Vector2 sailDirectionForward = new Vector2(forward.x, forward.z);

        float dotRight = Vector2.Dot(transform.right, WindManager.instance.wind.normalized);
        // float dotRight = Vector2.Dot(sailDirectionRight.normalized, WindManager.instance.wind.normalized);
        float dotForward = Vector2.Dot(transform.forward, WindManager.instance.wind.normalized);
        // float dotForward = Vector2.Dot(sailDirectionForward.normalized, WindManager.instance.wind.normalized);
        float rad = Mathf.Abs(transform.localRotation.y * Mathf.Deg2Rad * 10);
        
        rad = rad * 100;

        if (useForward)
            transform.RotateAround(mast.transform.position, transform.forward,
                Mathf.Sign(-dotRight) * WindManager.instance.windMagnitude * (1 - dotForward) * Time.deltaTime *
                windAttachmentFactor);
        else
            transform.RotateAround(mast.transform.position, transform.up,
                Mathf.Sign(-dotRight) * WindManager.instance.windMagnitude * (1 - dotForward) * Time.deltaTime *
                windAttachmentFactor * ((rope*100)-rad));
        
        if (rad > (rope*100))
        {
            
            if (useForward)
                transform.RotateAround(mast.transform.position, transform.forward,
                    Mathf.Sign(dotRight) * WindManager.instance.windMagnitude * (1 - dotForward) * Time.deltaTime *
                    adjustmentFactor * windAttachmentFactor);
            else
                transform.RotateAround(mast.transform.position, transform.up,
                    Mathf.Sign(dotRight) * WindManager.instance.windMagnitude * (1 - dotForward) * Time.deltaTime *
                    adjustmentFactor * windAttachmentFactor);
        }

        text.text = mySail + " Fwd: " + dotForward + " Right: " + dotRight;

        if (showRotText)
        {
            rotText.text = "On Ship: " + rad;
        }
    }
}