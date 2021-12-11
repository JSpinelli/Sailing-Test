using UnityEngine;

[ExecuteInEditMode]
public class FlagBehaviour : MonoBehaviour
{
    public LineRenderer _trail;
    public float lineResolution = 0.1f;
    private Vector3[] _positions;

    // Start is called before the first frame update
    void Start()
    {
        _positions = new Vector3[_trail.positionCount];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newWind =  transform.worldToLocalMatrix * new Vector3(WindManager.instance.wind.x, 0, WindManager.instance.wind.y).normalized;
        _positions[0] = transform.localPosition;
        for (int i = 1; i < _positions.Length; i++)
        {
            _positions[i] = _positions[i - 1] + (newWind * lineResolution);
            _positions[i].y = _positions[0].y;
        }
        _trail.SetPositions(_positions);
    }
}
