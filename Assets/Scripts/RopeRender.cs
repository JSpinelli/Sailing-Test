using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRender : MonoBehaviour
{
    public Transform attachmentPoint;
    private LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lr.SetPosition(0,transform.position);
        lr.SetPosition(1,attachmentPoint.position);
    }
}
