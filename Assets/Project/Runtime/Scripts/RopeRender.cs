using UnityAtoms.BaseAtoms;
using UnityEngine;

[ExecuteInEditMode]
public class RopeRender : MonoBehaviour
{
    public Transform target;
    public AnimationCurve tight;
    public AnimationCurve loose;

    private AnimationCurve currentCurve;

    public FloatReference ropeDiff;

    public bool lerpEnabled;
    public float tolerance = 1f;
    public int amountOfKeys;
    private Keyframe[] _animKeys;
    public LineRenderer _lineRenderer;
    public float magnitude;

    public float normalWidth = 0.2f;

    private void Start()
    {
        _animKeys = new Keyframe[amountOfKeys];
        currentCurve = tight;
    }

    private void LerpingCurvesFunction()
    {
        for (int i = 0; i < amountOfKeys; i++)
        {
            float time =(float) i / amountOfKeys;
            float lerpValue = Mathf.Abs(ropeDiff.Value) - tolerance;
            float value = Mathf.Lerp(tight.Evaluate(time), loose.Evaluate(time), lerpValue);
            _animKeys[i] = new Keyframe(time, value);
        }

        currentCurve = new AnimationCurve(_animKeys);
    }

    private void Update()
    {
        if (ropeDiff.Value > tolerance)
        {
            LerpingCurvesFunction();
        }        
        
        if (ropeDiff.Value < -tolerance)
        {
            _lineRenderer.startWidth = normalWidth / 2;
            _lineRenderer.endWidth = normalWidth / 2;
        }

        if (ropeDiff.Value < tolerance && ropeDiff > -tolerance)
        {
            _lineRenderer.startWidth = normalWidth;
            _lineRenderer.endWidth = normalWidth;
        }


        if (lerpEnabled)
            currentCurve = loose;

        float distance = Vector3.Distance(target.position,transform.position);
        Vector3 direction = (target.position - transform.position).normalized;
        for (int i = 0; i < _lineRenderer.positionCount-1; i++)
        {
            float step = ((float)i / _lineRenderer.positionCount);
            Vector3 pos = transform.position + (direction * ((step) * distance));
            pos.y = pos.y + currentCurve.Evaluate(step) * magnitude;
            _lineRenderer.SetPosition(i, pos);
        }
        _lineRenderer.SetPosition(_lineRenderer.positionCount-1,target.position);
    }
}