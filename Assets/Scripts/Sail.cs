using UnityAtoms.BaseAtoms;
using UnityEngine;

public class Sail : MonoBehaviour
{
    public Transform ship;
    public GameObject mast;
    public float adjustmentFactor = 2f;
    public float windAttachmentFactor = 1.5f;
    public FloatReference rope;

    private void Update()
    {
        float dotRight = Vector2.Dot(transform.right, WindManager.instance.wind.normalized);

        float dotForward = Vector2.Dot(transform.forward, WindManager.instance.wind.normalized);

        float windDirectionShip = Vector2.Dot(ship.right, WindManager.instance.wind.normalized);
        float sailDirectionShip = Vector2.Dot(ship.forward, transform.right);
        

        float angle = Vector3.Angle(transform.forward, ship.forward);
        
        // if ((Mathf.Sign(windDirectionShip) > 0) && (Mathf.Sign(sailDirectionShip) < 0))
        // {
        //     Debug.Log("Wrong Side");
        // }
        //
        // if ((Mathf.Sign(windDirectionShip) > 0) && (Mathf.Sign(sailDirectionShip) < 0))
        // {
        //     Debug.Log("Wrong Side");
        // }


        if (angle > rope.Value)
        {
            transform.RotateAround(mast.transform.position, transform.up,
                (Mathf.Sign(-transform.localRotation.y) * adjustmentFactor) / WindManager.instance.windMagnitude);
        }
        else
        {
            transform.RotateAround(mast.transform.position, transform.up,
                -Mathf.Sign(dotRight) * WindManager.instance.windMagnitude * (1 - dotForward) *
                windAttachmentFactor * (Mathf.Abs((rope.Value - angle) / rope.Value)));
        }
    }
}