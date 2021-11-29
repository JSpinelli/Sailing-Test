using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterManager : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private Material waves;
    private static readonly int Direction = Shader.PropertyToID("_FoamDirection");

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        waves = GetComponent<MeshRenderer>().sharedMaterial;
    }

    private void Update()
    {
        // Vector2 direction = Vector2.left - WindManager.instance.wind;
        // float angle = Mathf.Atan2(direction.y,  direction.x) * Mathf.Rad2Deg;
        // if (angle < 0f) angle += 360f;
        // waves.SetFloat(Direction, ((angle/360)*2)-1);
        MeshUpdate();
    }
    
    private void MeshUpdate()
    { 
        Vector3[] vertices = _meshFilter.mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = WaveManager.instance.GetWaveHeight(transform.position.x + vertices[i].x, transform.position.z + vertices[i].z);
        }
        _meshFilter.mesh.vertices = vertices;
        _meshFilter.mesh.RecalculateBounds();
        _meshFilter.mesh.RecalculateNormals();
    }
}
