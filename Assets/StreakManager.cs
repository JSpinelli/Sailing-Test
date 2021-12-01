using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreakManager : MonoBehaviour
{
    private TrailRenderer trail;
    private Vector3[] positions;

    // Start is called before the first frame update
    void Start()
    {
        trail = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        positions = new Vector3[trail.positionCount];
        trail.GetPositions(positions);
        float yPos = transform.position.y;
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i].y = yPos;
        }
        trail.SetPositions(positions);
    }
}