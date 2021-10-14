using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindParticles : MonoBehaviour
{
    public ParticleSystem ps;
    // Update is called once per frame
    void Update()
    {
        Vector3 newWind = new Vector3(WindManager.instance.wind.x, 0, WindManager.instance.wind.y);
        var emission = ps.emission;
        emission.rateOverTime = WindManager.instance.windMagnitude / 10;
        transform.forward = newWind;
    }
}
