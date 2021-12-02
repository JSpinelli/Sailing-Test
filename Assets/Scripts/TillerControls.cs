using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TillerControls : MonoBehaviour
{
    public Material baseMat;
    public Material grabbedMat;

    private MeshRenderer _myMeshRenderer;

    private bool prevState = false;
    // Start is called before the first frame update
    void Start()
    {
        _myMeshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (PlayerController.tillerGrabbed && !prevState)
        {
            prevState = true;
            GrabbedTiller();
        }

        if (!PlayerController.tillerGrabbed && prevState)
        {
            prevState = false;
            ReleasedTiller();
        }
    }

    public void GrabbedTiller()
    {
        _myMeshRenderer.material = grabbedMat;
    }

    public void ReleasedTiller()
    {
        _myMeshRenderer.material = baseMat;
    }
}
