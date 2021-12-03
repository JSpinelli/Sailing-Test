using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{

    public int myIndex = 0;

    public MeshRenderer point1;
    public MeshRenderer point2;
    public AudioSource source;
    private bool wentThrough = false;
    private float timer = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (!wentThrough)
        {
            TutorialIslandManager.Instance.UpdateRing(myIndex);
            wentThrough = true;
            if (source !=null)
                source.Play();
            timer = 0;
        }
    }

    private void Update()
    {
        if (wentThrough)
        {
            if (timer < TutorialIslandManager.Instance.timeToDeactivateGate)
            {
                timer += Time.deltaTime;
            }
            else
            {
                if (!point1.isVisible && !point2.isVisible)
                    gameObject.SetActive(false);
            }
        }
    }
}
