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
        var main = ps.main;
        //emission.rateOverTime = WindManager.instance.windMagnitude / 10;
        transform.up = newWind;
        //transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(newWind), 0);
        main.startSpeed = WindManager.instance.windMagnitude/15;
    }
}
