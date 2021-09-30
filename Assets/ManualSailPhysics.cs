using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ManualSailPhysics : MonoBehaviour
{
    // Update is called once per frame
    public GameObject mast;

    public float max;
    void Update()
    {
        Vector3 dir = new Vector3(WindManager.instance.wind.x, 0, WindManager.instance.wind.y);
        if (transform.eulerAngles.y > max && transform.eulerAngles.y < (360-max))
            return;
        transform.RotateAround(mast.transform.position, Vector3.up, -20 * Time.deltaTime);
       // Debug.Log(transform.eulerAngles.y);
        
    }
}
