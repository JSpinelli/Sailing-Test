using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TutorialIslandManager : MonoBehaviour
{
    public static TutorialIslandManager Instance;
    
    public List<GameObject> rings;
    private bool _finishedRings = false;
    public GameObject island;
    public Vector2 startingWind;
    public float startingMagnitude;
    public float timeToDeactivateGate;

    public float initialXLenght;
    public float initialZLenght;
    public float initialXAmp;
    public float initialZAmp;

    public float ampIncrease;

    public TextMeshProUGUI text;

    public StringReference pointOfSailing;
    
    public bool startTutorial = true;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Should not be another class");
            Destroy(this);
        }
    }
    void Start()
    {
        if (startTutorial)
        {
            GameManager.Instance.autoFrontSailPositioning = true;
            GameManager.Instance.autoMainSailPositioning = true;
            UIManager.Instance.SetActiveFrontSailControls(false);
            UIManager.Instance.SetActiveMainSailControls(false);
        }
        island.SetActive(false);
        WindManager.instance.SetWind(startingWind,startingMagnitude);
        foreach (var ring in rings)
        {
            ring.SetActive(false);
        }
        rings[0].SetActive(true);
        WaveManager.instance.ChangeWaveValues(initialXAmp,initialXLenght,initialZAmp,initialZLenght);
        UIManager.Instance.PointOfSailingViz(false);
    }

    private void Update()
    {
        if (pointOfSailing.Value == "In Irons")
        {
            text.gameObject.SetActive(true);
            text.text = " I can't sail towards the wind, I need to adjust my position ";
        }
        else
        {
            text.gameObject.SetActive(false);
            text.text = "";
        }
    }

    public void UpdateRing(int index)
    {
        if (index >= rings.Count) return;
        if ((index) == 2)
        {
            GameManager.Instance.autoMainSailPositioning = false;
            UIManager.Instance.SetActiveMainSailControls(true);
        }
        if ((index + 1) >= rings.Count)
        {
            GameManager.Instance.autoMainSailPositioning = false;
            UIManager.Instance.SetActiveMainSailControls(true);
            _finishedRings = true;
            GameManager.Instance.autoFrontSailPositioning = false;
            UIManager.Instance.SetActiveFrontSailControls(true);
            UIManager.Instance.PointOfSailingViz(true);
            WindManager.instance.SetWind(new Vector2(0.2f,-0.1f).normalized,startingMagnitude+1);
            island.SetActive(true);
        }
        else
        {
            WaveManager.instance.ChangeWaveValues(initialXAmp+ampIncrease,initialXLenght,initialZAmp+ampIncrease,initialZLenght);
            rings[index+1].SetActive(true);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TutorialIslandManager))]
public class DrawTutorialIslandManager: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TutorialIslandManager manager = (TutorialIslandManager)target;
        if(GUILayout.Button("Finish Tutorial"))
        {
            manager.UpdateRing(manager.rings.Count-1);
        }
    }
}
#endif
