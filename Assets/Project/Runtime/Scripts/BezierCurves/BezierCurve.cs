using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

[ExecuteInEditMode]
public class BezierCurve : MonoBehaviour
{
    public Vector3 startPoint = new Vector3(-0.0f, 0.0f, 0.0f);
    public Vector3 endPoint = new Vector3(-2.0f, 2.0f, 0.0f);
    public Vector3 startTangent = Vector3.zero;
    public Vector3 endTangent = Vector3.zero;

    private List<float> lookUpTableTime = new List<float>();
    public List<Vector3> lookUpTableDistance = new List<Vector3>();
    private int currentStep = 0;

    [HideInInspector] public float myLength;

    private Vector3 previousParentPos = Vector3.zero;

    public void SetValues(Vector3 position)
    {
        startPoint = position;
        endPoint = position + new Vector3(0.0f, 1f, 0f);
        endTangent = position + new Vector3(1f, 1f, 1f);
    }

    public void CalculateLuT(float stepDistance)
    {
        float lengthPercentage = 0;
        float partialDist = 0;
        Vector3 pos = startPoint;
        while (lengthPercentage < 1)
        {
            lookUpTableTime.Add(partialDist);
            Vector3 tempPos = CalculatePosition(lengthPercentage);
            float dist = Vector3.Distance(pos, tempPos);
            partialDist = partialDist + dist;
            //Best way to measure distances, smaller doesn't affect the result, bigger has too much of an error
            lengthPercentage += 0.01f;
            pos = tempPos;
        }

        lookUpTableTime.Add(partialDist);
        myLength = partialDist;

        int numberOfSteps = (int) Mathf.Floor(myLength / stepDistance);
        int n = lookUpTableTime.Count;
        float currentDist = 0;
        for (int i = 0; i < numberOfSteps; i++)
        {
            int j = 0;
            while (j < n - 1)
            {
                if (currentDist >= lookUpTableTime[j] && currentDist <= lookUpTableTime[j + 1])
                {
                    float val = currentDist.Remap(lookUpTableTime[j], lookUpTableTime[j + 1], j / (n - 1f),
                        (j + 1) / (n - 1f));
                    lookUpTableDistance.Add(CalculatePosition(val));
                }

                j++;
            }

            currentDist = currentDist + stepDistance;
        }
    }

    public Vector3 CalculatePosition(float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * (startPoint + transform.position);
        p += 3 * uu * t * (startTangent + transform.position);
        p += 3 * u * tt * (endTangent + transform.position);
        p += ttt * (endPoint + transform.position);

        return p;
    }

    public Vector3 StartPos()
    {
        return startPoint + transform.position;
    }

    public Vector3 EndPos()
    {
        return endPoint + transform.position;
    }


    public void UpdatePos()
    {
        if (previousParentPos == transform.parent.position)
            return;

        Vector3 diff = transform.parent.position - previousParentPos;
        startPoint = diff + startPoint;
        endPoint = diff + endPoint;
        startTangent = diff + startTangent;
        endTangent = diff + endTangent;
        previousParentPos = transform.parent.position;
    }

    public void ResetPos()
    {
        Vector3 diff = previousParentPos;
        startPoint = diff + startPoint;
        endPoint = diff + endPoint;
        startTangent = diff + startTangent;
        endTangent = diff + endTangent;
        previousParentPos = transform.parent.position;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(BezierCurve))]
public class DrawBezierCurve : Editor
{
    private void OnSceneViewGUI(SceneView sv)
    {
        BezierCurve be = target as BezierCurve;

        Vector3 worldStartPoint = be.transform.position + be.startPoint;
        Vector3 worldEndPoint = be.transform.position + be.endPoint;
        Vector3 worldStartTangent = be.transform.position + be.startTangent;
        Vector3 worldEndTangent = be.transform.position + be.endTangent;

        worldStartPoint = Handles.PositionHandle(worldStartPoint, Quaternion.identity);
        worldEndPoint = Handles.PositionHandle(worldEndPoint, Quaternion.identity);
        worldStartTangent = Handles.PositionHandle(worldStartTangent, Quaternion.identity);
        worldEndTangent = Handles.PositionHandle(worldEndTangent, Quaternion.identity);
        Handles.DrawBezier(worldStartPoint, worldEndPoint, worldStartTangent, worldEndTangent, Color.red, null, 2f);

        be.startPoint = worldStartPoint - be.transform.position;
        be.endPoint = worldEndPoint - be.transform.position;
        be.startTangent = worldStartTangent - be.transform.position;
        be.endTangent = worldEndTangent - be.transform.position;
    }

    void OnEnable()
    {
        Tools.current = Tool.None;
        SceneView.duringSceneGui += OnSceneViewGUI;
    }

    void OnDisable()
    {
        Tools.current = Tool.Move;
        SceneView.duringSceneGui -= OnSceneViewGUI;
    }
}
#endif