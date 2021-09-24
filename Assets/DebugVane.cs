using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugVane : MonoBehaviour
{
    public BoatManager bm;
    void Update()
    {
        Vector3 newdir = new Vector3(bm.debuggingForward.x, 0, bm.debuggingForward.z);
        transform.forward = newdir;
    }
}
