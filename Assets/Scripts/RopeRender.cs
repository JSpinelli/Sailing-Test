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
        Vector3 dir = (transform.position - attachmentPoint.position).normalized;
        Debug.Log(dir);
        positionsSin[0] = transform.position;
        for (int i = 1; i < SinSteps-1; i++)
        {
            float step = (float) i / SinSteps;
            Vector3 newPoint = transform.position + (step * dir);
            if (notTight)
            {
                newPoint.z = newPoint.z + Mathf.Sin(newPoint.z * WaveScale);
                //newPoint.x = newPoint.x + Mathf.Sin(newPoint.x * WaveScale);
                newPoint.y = newPoint.y + Mathf.Sin(newPoint.y * WaveScale);
            }
            positionsSin[i] = newPoint;
        }
        positionsSin[SinSteps-1] = attachmentPoint.position;
        lr.SetPositions(positionsSin);
    }
}