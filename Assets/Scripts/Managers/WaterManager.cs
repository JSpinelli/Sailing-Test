using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterManager : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Mesh _mesh;
    private bool _updatingPosition;
    private NativeArray<Vector3> _vertices;
    private MeshWave _job;
    private JobHandle _handle;

    public GameObject floaterPrefab;
    public GameObject myFloaters;
    public float chanceOfSpawn = 0.3f;
    private bool _floatersActive = false;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _mesh = _meshFilter.mesh;
        _mesh.MarkDynamic();
    }

    private void Start()
    {
        _vertices = new NativeArray<Vector3>(_meshFilter.mesh.vertices, Allocator.Persistent);
        if (myFloaters == null)
        {
            _floatersActive = false;
        }
        else
        {
            _floatersActive = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _updatingPosition = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _updatingPosition = false;
    }

    private void Update()
    {
        if (_updatingPosition && _meshRenderer.isVisible)
        {
            MeshUpdate();
            if (_floatersActive)
                myFloaters.SetActive(true);
        }
        else
        {
            if (_floatersActive)
                myFloaters.SetActive(false);
        }
    }

    private void MeshUpdate()
    {
        _job = new MeshWave
        {
            Vertices = _vertices,
            Position = transform.position,
        };
        _handle = _job.Schedule(_meshFilter.mesh.vertices.Length, 128);
        _handle.Complete();
        _mesh.vertices = _vertices.ToArray();
        //_mesh.RecalculateBounds();
        //_mesh.RecalculateNormals();
    }

    private void OnDestroy()
    {
        _vertices.Dispose();
    }

    public void SpawnFloaters()
    {
        if (myFloaters != null)
        {
            DestroyImmediate(myFloaters);
        }

        myFloaters = new GameObject("Floaters");
        myFloaters.transform.parent = transform;
        var meshFilter = GetComponent<MeshFilter>();
        var mesh = meshFilter.sharedMesh;
        foreach (var vert in mesh.vertices)
        {
            if (Random.Range(0f, 1f) < chanceOfSpawn)
            {
                Instantiate(floaterPrefab, transform.position + vert, Quaternion.identity, myFloaters.transform);
            }
        }
    }
}

public struct MeshWave : IJobParallelFor
{
    public NativeArray<Vector3> Vertices;

    public Vector3 Position;

    public void Execute(int i)
    {
        Vertices[i] = new Vector3(
            Vertices[i].x,
            WaveManager.instance.GetWaveHeight(Position.x + Vertices[i].x, Position.z + Vertices[i].z),
            Vertices[i].z);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(WaterManager))]
public class DrawWaterManager : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WaterManager manager = (WaterManager) target;
        if (GUILayout.Button("Add Floaters"))
        {
            manager.SpawnFloaters();
        }
    }
}
#endif