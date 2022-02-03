using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class BezierCurveEnd : BezierCurve
{
    public BezierCurve previous;
    public BezierCurve end;
    public float magnitudeStart = 1f;
    public float magnitudeEnd = 1f;

    public void SetEnd(BezierCurve previous, BezierCurve end)
    {
        this.previous = previous;
        this.end = end;
        this.startPoint = previous.endPoint;
        this.endPoint = end.startPoint;
        this.startTangent = this.startPoint + (Vector3.Normalize(previous.endPoint - previous.endTangent) * magnitudeStart);
        this.endTangent = this.endPoint + (Vector3.Normalize(this.endPoint - end.startTangent) * magnitudeEnd);
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(BezierCurveEnd))]
public class DrawBezierCurveEnd : Editor
{
    private void OnSceneViewGUI(SceneView sv)
    {
        BezierCurveEnd bc = target as BezierCurveEnd;

        Vector3 worldStartPoint = bc.transform.position + bc.startPoint;
        Vector3 worldEndPoint = bc.transform.position +  bc.endPoint;
        Vector3 worldStartTangent = bc.transform.position + bc.startTangent;
        Vector3 worldEndTangent = bc.transform.position +  bc.endTangent;
        
        worldStartTangent = Handles.PositionHandle(worldStartTangent, Quaternion.identity);
        worldEndTangent = Handles.PositionHandle(worldEndTangent, Quaternion.identity);
        Handles.DrawBezier(worldStartPoint, worldEndPoint, worldStartTangent, worldEndTangent, Color.red, null, 2f);
        
        bc.startPoint = bc.previous.endPoint;
        bc.endPoint = bc.end.startPoint;
        bc.startTangent =  worldStartTangent - bc.transform.position;
        bc.endTangent = worldEndTangent - bc.transform.position;
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
