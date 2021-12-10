using UnityAtoms.BaseAtoms;
using UnityEngine;

public class Sail : MonoBehaviour
{
    public Transform ship;
    public GameObject mast;
    public float adjustmentFactor = 20f;
    public float windAttachmentFactor = 15f;
    public FloatReference rope;

    public float tolerance = 2;

    private float _diff;
    private float _angle;
    private float _windDirectionShip;
    private float _yRot;

    private void Update()
    {
        _windDirectionShip = Vector2.Dot(ship.right, WindManager.instance.wind.normalized);
        _angle = Vector3.Angle(transform.forward, ship.forward);
        _diff = rope.Value - _angle;
        _yRot = transform.localRotation.y;
        
        // WENT TO FAR
        if (_diff < -tolerance)
        {
            transform.RotateAround(mast.transform.position, transform.up,
                ((Mathf.Sign( -_yRot) * adjustmentFactor) * WindManager.instance.windMagnitude) * Time.deltaTime);
            return;
        }
        
        // WRONG SIDE
        if ((Mathf.Sign(_windDirectionShip) * Mathf.Sign(_yRot)> 0))
        {
            transform.RotateAround(mast.transform.position, transform.up,
                (Mathf.Sign(-_windDirectionShip) * windAttachmentFactor) * WindManager.instance.windMagnitude * Time.deltaTime);
            return;
        }
        
        // GOING WITH THE WIND
        if (_diff > tolerance)
        {
            transform.RotateAround(mast.transform.position, transform.up,
                ((Mathf.Sign(-_windDirectionShip) * windAttachmentFactor) * WindManager.instance.windMagnitude) * Time.deltaTime);
        }
    }
}