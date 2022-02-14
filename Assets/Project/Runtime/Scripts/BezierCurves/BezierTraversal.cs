using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

public class BezierTraversal : MonoBehaviour
{
    // public BezierCurveBuilder path;
    // public GameObject movableThing;
    // public float speed = 1f;
    // public float stepDistance = 0.1f;
    // public float stepTolerance = 0.1f;
    // public float startTime;
    // public float endTime;
    // public bool loop;
    //
    // private bool _moving;
    // private bool _topSide = true;
    //
    // private List<Vector3> lookUpTablePoints = new List<Vector3>();
    // private int _lookUpIndex = 0;
    // private float totalLength = 0;
    //
    // private void Start()
    // {
    //     if (transform.position.y < 0)
    //     {
    //         _topSide = false;
    //     }
    //     
    //     
    //     foreach (var curve in path.curve)
    //     {
    //         curve.CalculateLuT(stepDistance);
    //         totalLength = totalLength + curve.myLength;
    //         for (int i = 1; i < curve.lookUpTableDistance.Count; i++)
    //         {
    //             lookUpTablePoints.Add(curve.lookUpTableDistance[i]); 
    //         }
    //     }
    //     lookUpTablePoints.Insert(0,path.curve[0].lookUpTableDistance[0]);
    //
    //     for (int i = 0; i < lookUpTablePoints.Count -1 ; i++)
    //     {
    //         Debug.Log(Vector3.Distance(lookUpTablePoints[i],lookUpTablePoints[i+1]));
    //     }
    //     float timeToMove = (endTime - startTime) * GameManager.Instance.secondsToHour;
    //     speed = totalLength / timeToMove;
    // }
    //
    // private void FixedUpdate()
    // {
    //     if (path.curve.Count == 0) return;
    //     if (GameManager.Instance.goingForward && startTime == GameManager.Instance.hour ||
    //         (!GameManager.Instance.goingForward && endTime == GameManager.Instance.hour))
    //     {
    //         _moving = true;
    //     }
    //
    //     if (_moving)
    //     {
    //         if (GameManager.Instance.goingForward && _topSide)
    //         {
    //             MoveForward();
    //         }
    //         else
    //         {
    //             MoveBackward();
    //         }
    //     }
    // }
    //
    // private void MoveForward()
    // {
    //     if (_lookUpIndex == lookUpTablePoints.Count)
    //     {
    //         if (loop)
    //         {
    //             _lookUpIndex = 0;
    //         }
    //         else
    //         {
    //             Debug.Log(gameObject.name + " finished with time " + GameManager.Instance.hour + " : " +
    //                       GameManager.Instance.timer);
    //             _moving = false;
    //             _lookUpIndex--;
    //         }
    //     }
    //     else
    //     {
    //         if (Vector3.Distance(movableThing.transform.position, lookUpTablePoints[_lookUpIndex]) > stepTolerance)
    //         {
    //             Vector3 objective =
    //                 Vector3.MoveTowards(movableThing.transform.position, lookUpTablePoints[_lookUpIndex],
    //                     speed* Time.fixedDeltaTime);
    //             movableThing.transform.position = objective;
    //         }
    //         else
    //         {
    //             _lookUpIndex++;
    //         }
    //     }
    // }
    //
    // private void MoveBackward()
    // {
    //     if (_lookUpIndex < 0)
    //     {
    //         if (loop)
    //         {
    //             _lookUpIndex = lookUpTablePoints.Count - 1;
    //         }
    //         else
    //         {
    //             Debug.Log(gameObject.name + " finished with time " + GameManager.Instance.hour + " : " +
    //                       GameManager.Instance.timer);
    //             _moving = false;
    //             _lookUpIndex++;
    //         }
    //     }
    //     else
    //     {
    //         if (Vector3.Distance(movableThing.transform.position, lookUpTablePoints[_lookUpIndex]) > stepTolerance)
    //         {
    //             Vector3 objective =
    //                 Vector3.MoveTowards(movableThing.transform.position, lookUpTablePoints[_lookUpIndex],
    //                     speed * Time.fixedDeltaTime);
    //             movableThing.transform.position = objective;
    //         }
    //         else
    //         {
    //             _lookUpIndex--;
    //         }
    //     }
    // }
}
#if UNITY_EDITOR
[CustomEditor(typeof(BezierTraversal))]
public class DrawBezierTraversal : Editor
{
    // public override void OnInspectorGUI()
    // {
    //     DrawDefaultInspector();
    //
    //     BezierTraversal mover = (BezierTraversal) target;
    //     if (GUILayout.Button("Set object at start"))
    //     {
    //         mover.movableThing.transform.position = mover.path.curve[0].StartPos();
    //     }
    // }
}
#endif