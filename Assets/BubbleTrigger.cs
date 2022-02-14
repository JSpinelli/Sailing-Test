using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PostProcessingManager.Instance.EnableSeaFilter();
    }

    private void OnTriggerExit(Collider other)
    {
        PostProcessingManager.Instance.DisableSeaFilter();
    }
}
