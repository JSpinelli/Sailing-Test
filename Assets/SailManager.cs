using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            gameObject.transform.Rotate(0,-1,0);
        }
        if (Input.GetKey(KeyCode.C))
        {
            gameObject.transform.Rotate(0,1,0);
        }
    }
}
