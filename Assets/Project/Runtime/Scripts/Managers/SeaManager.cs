using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class SeaManager : MonoBehaviour
{
    public GameObject myOceanTiles;
    public GameObject boat;

    private float xOffset;
    private float zOffset;

    public int amountXTiles;
    public int amountZTiles;
    public GameObject tile;
    public int tileSize;
    
    public void PlaceTiles()
    {
        DestroyTiles();
        myOceanTiles = new GameObject("My Ocean Tiles");
        myOceanTiles.transform.parent = transform;
        
        float positionX = (-amountXTiles * tileSize) / 2;
        for (int i = 0; i < amountXTiles; i++)
        {
            float positionZ = -(amountZTiles * tileSize)/ 2;
            for (int j = 0; j < amountZTiles; j++)
            {
                Instantiate(tile, new Vector3(positionX, 0, positionZ), Quaternion.identity, myOceanTiles.transform);
                positionZ += tileSize;
            }
            positionX += tileSize;
        }
    }

    public void DestroyTiles()
    {
        if (myOceanTiles == null) return;
        DestroyImmediate(myOceanTiles);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SeaManager))]
public class DrawSeaManager : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SeaManager manager = (SeaManager) target;
        if (GUILayout.Button("Position Sea Planes"))
        {
            manager.PlaceTiles();
        }
        
        if (GUILayout.Button("Destroy Tiles"))
        {
            manager.DestroyTiles();
        }
    }
}
#endif