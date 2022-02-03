using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{

    public Transform target;

    private Vector3 _offset;
    // Start is called before the first frame update
    void Start()
    {
        _offset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = new Vector3(target.position.x, 0, target.position.z) + _offset;
        transform.position = position;
    }
}
