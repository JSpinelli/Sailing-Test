using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vane : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 newWind = new Vector3(WindManager.instance.wind.x, 0, WindManager.instance.wind.y);
        transform.forward = newWind;
    }
}
