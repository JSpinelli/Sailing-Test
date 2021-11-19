using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class InteractionBehaviour : MonoBehaviour
{
    public CinemachineVirtualCamera cam;

    public float fovZoom;
    public float normalFoV;
    public float viewDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.looking)
        {
            cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, fovZoom, Time.deltaTime);
            RaycastHit[] hits= Physics.RaycastAll(transform.position, transform.forward, viewDistance);
            if (hits.Length > 0)
                Debug.Log(hits[0].transform.name);
        }
        else
        {
            cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, normalFoV, Time.deltaTime);
        }

        if (PlayerController.interactionButton)
        {
            
        }

    }
}
