using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class ManualSailPhysics : MonoBehaviour
{
    // Update is called once per frame
    public GameObject mast;

    public Transform forward1;
    public Transform forward2;
    public Transform right1;
    public Transform right2;

    public float adjustmentFactor = 2f;

    [Range(0.0001f,0.15f)]
    public float rope;
    public MeshRenderer sail;
    public Material defaultMat;
    public Material correctingMat;

    public TextMeshProUGUI text;
    public string mySail;

    public bool showRotText = false;
    public TextMeshProUGUI rotText;
    void Update()
    {
        Vector3 right = right2.position - right1.position;
        Vector3 forward = forward2.position - forward1.position;
        Vector2 sailDirectionRight = new Vector2(right.x, right.z);
        Vector2 sailDirectionForward = new Vector2(forward.x, forward.z);
        
        float dotRight = Vector2.Dot(sailDirectionRight.normalized, WindManager.instance.wind.normalized);
        float dotForward = Vector2.Dot(sailDirectionForward.normalized, WindManager.instance.wind.normalized);
        transform.RotateAround(mast.transform.position, transform.up,  Mathf.Sign(-dotRight) * WindManager.instance.windMagnitude * (1- dotForward) * Time.deltaTime);
        float rad = Mathf.Abs(transform.localRotation.y * Mathf.Deg2Rad * 10);
        //sail.material = defaultMat;
        if (rad > rope)
        {
            transform.RotateAround(mast.transform.position, transform.up,  Mathf.Sign(dotRight) * WindManager.instance.windMagnitude * (1- dotForward) * Time.deltaTime * adjustmentFactor);
            //sail.material = correctingMat;
        }
        text.text = mySail + " Fwd: " + dotForward + " Right: " + dotRight;

        if (showRotText)
        {
            rotText.text = "On Ship: " + rad;
        }

    }
}
