using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vane : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 newWind = new Vector3(0, 0, 1);
        transform.forward = newWind;
    }
}
