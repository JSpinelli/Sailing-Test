using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class BezierCurveContinuation : BezierCurve
{
    public BezierCurve previous;
    public float magnitude = 1f;

    public void SetPrevious(BezierCurve previous)
    {
        this.previous = previous; 
        this.startPoint = previous.endPoint;
        this.endPoint = previous.endPoint + new Vector3(0.0f,1f,1f);
        this.startTangent =this.startPoint +  (Vector3.Normalize(previous.endPoint - previous.endTangent) * magnitude);
        this.endTangent = previous.endPoint + new Vector3(1f,1f,1f);
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(BezierCurveContinuation))]
public class DrawBezierCurveContinuation : Editor
{
    private void OnSceneViewGUI(SceneView sv)
    {
        BezierCurveContinuation bc = target as BezierCurveContinuation;

        Vector3 worldStartPoint = bc.transform.position + bc.startPoint;
        Vector3 worldEndPoint = bc.transform.position +  bc.endPoint;
        Vector3 worldStartTangent = bc.transform.position + bc.startTangent;
        Vector3 worldEndTangent = bc.transform.position +  bc.endTangent;
        
        worldEndPoint = Handles.PositionHandle(worldEndPoint, Quaternion.identity);
        worldStartTangent = Handles.PositionHandle(worldStartTangent, Quaternion.identity);
        worldEndTangent = Handles.PositionHandle(worldEndTangent, Quaternion.identity);
        Handles.DrawBezier(worldStartPoint, worldEndPoint, worldStartTangent, worldEndTangent, Color.red, null, 2f);

        bc.startPoint = bc.previous.endPoint;
        bc.endPoint = worldEndPoint - bc.transform.position;
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
