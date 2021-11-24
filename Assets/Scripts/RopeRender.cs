using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RopeRender : MonoBehaviour
{
    public Transform attachmentPoint;
    private LineRenderer lr;

    public bool notTight;
    public int SinSteps;
    public float WaveScale;

    private Vector3[] positionsSin;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        positionsSin = new Vector3[SinSteps];
    }


    private void Update()
    {
        lr.SetPosition(0,transform.position);
        lr.SetPosition(1,attachmentPoint.position);
    }
}