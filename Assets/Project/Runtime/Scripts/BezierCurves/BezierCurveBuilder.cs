using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BezierCurveBuilder : MonoBehaviour
{
    public List<BezierCurve> curve;

    public GameObject bezierCurve;
    public GameObject bezierCurveContinuation;
    public GameObject bezierCurveEnd;

    private Vector3 prevPos;

    [HideInInspector]
    public bool toggleShow = false;

    public void AddSection()
    {

        if (curve.Count == 0)
        {
            GameObject prefab = Instantiate(bezierCurve, transform.position, Quaternion.identity,transform);
            prefab.transform.localPosition = Vector3.zero;
            BezierCurve firstSection = prefab.GetComponent<BezierCurve>();
            firstSection.SetValues(transform.position);
            curve.Add(firstSection);
        }
        else
        {
            BezierCurve previous = curve[curve.Count - 1];
            GameObject prefab = Instantiate(bezierCurveContinuation, previous.endPoint, Quaternion.identity,transform);
            prefab.transform.localPosition = Vector3.zero;
            BezierCurveContinuation nextTrack = prefab.GetComponent<BezierCurveContinuation>();
            nextTrack.SetPrevious(previous);
            curve.Add(nextTrack);
        }
    }

    public void CloseLoop()
    {
            BezierCurve previous = curve[curve.Count - 1];
            BezierCurve first = curve[0];
            GameObject prefab = Instantiate(bezierCurveEnd, previous.endPoint, Quaternion.identity,transform);
            prefab.transform.localPosition = Vector3.zero;
            BezierCurveEnd end = prefab.GetComponent<BezierCurveEnd>();
            end.SetEnd(previous, first);
            curve.Add(end);
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(BezierCurveBuilder))]
public class DrawBezierCurveBuilder : Editor
{
    private bool isClosed = false;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BezierCurveBuilder curveBuilder = (BezierCurveBuilder)target;
        if(GUILayout.Button("Add Section"))
        {
            if (!isClosed){
                curveBuilder.AddSection();
            }
            
        }
        if(GUILayout.Button("Close Loop"))
        {
            isClosed = true;
            curveBuilder.CloseLoop();
        }
        curveBuilder.toggleShow = GUILayout.Toggle(curveBuilder.toggleShow,"Always Show Line");

    }
    private void OnSceneViewGUI(SceneView sv)
    {
        BezierCurveBuilder bb = target as BezierCurveBuilder;
        Vector3 myPos = bb.transform.position;
        if (bb.curve.Count == 0) return;
        for (int i=0; i< bb.curve.Count ; i++){
            Handles.DrawBezier(bb.curve[i].startPoint + myPos, bb.curve[i].endPoint + myPos, bb.curve[i].startTangent + myPos ,bb.curve[i].endTangent + myPos, Color.blue, null, 4f);
        }
    }

    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneViewGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneViewGUI;
    }

}
#endif
