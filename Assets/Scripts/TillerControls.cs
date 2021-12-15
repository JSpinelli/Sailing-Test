using UnityAtoms.BaseAtoms;
using UnityEngine;

public class TillerControls : MonoBehaviour
{
    public Material baseMat;
    public Material grabbedMat;
    public Material centeredTiller;
    public Transform tillerOrigin;
    public AudioSource centeredSound;

    public AnimationCurve tillerVelocity;

    public FloatReference turningFactor;
    public FloatReference tillerPos;
    public FloatReference tillerSensitivity;

    private MeshRenderer _myMeshRenderer;

    private bool _prevState;
    private bool _prevPos;

    void Start()
    {
        _myMeshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (PlayerController.tillerGrabbed)
        {
            if (((transform.localRotation.y >= -0.1f)&&(transform.localRotation.y <= 0.1f)) && (_prevPos || !_prevState))
            {
                _prevPos = false;
                CenteredTiller();
                
                //Only play sound if you are coming from movement not if you just grabbed the tiller
                if (_prevState)
                {
                    centeredSound.Stop();
                    centeredSound.Play(); 
                }
            }
            if (((transform.localRotation.y < -0.1f)||(transform.localRotation.y > 0.1f))  && (!_prevPos || !_prevState))
            {
                _prevPos = true;
                GrabbedTiller();
            }
            _prevState = true;
        }

        if (!PlayerController.tillerGrabbed && _prevState)
        {
            _prevState = false;
            ReleasedTiller();
        }

        TillerUpdate();
    }

    private void TillerUpdate()
    {
        if (PlayerController.tillerDir.x > 0 &&
            (transform.localRotation.eulerAngles.y < 80 || transform.localRotation.eulerAngles.y > 275))
        {
            transform.RotateAround(tillerOrigin.position, tillerOrigin.up,
                PlayerController.tillerDir.x * tillerSensitivity.Value);
        }

        if (PlayerController.tillerDir.x < 0 &&
            (transform.localRotation.eulerAngles.y > 280 || transform.localRotation.eulerAngles.y < 85))
        {
            transform.RotateAround(tillerOrigin.position, tillerOrigin.up,
                PlayerController.tillerDir.x * tillerSensitivity.Value);
        }

        float tillerVal = Mathf.Sign(transform.localRotation.y) *
                          tillerVelocity.Evaluate(Mathf.Abs(transform.localRotation.y));

        tillerPos.Value = tillerVal * turningFactor.Value;
    }

    private void GrabbedTiller()
    {
        _myMeshRenderer.material = grabbedMat;
    }

    private void CenteredTiller()
    {
        _myMeshRenderer.material = centeredTiller;
    }

    private void ReleasedTiller()
    {
        _myMeshRenderer.material = baseMat;
    }
}