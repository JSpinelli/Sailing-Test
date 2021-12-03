using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        island.SetActive(false);
        WindManager.instance.SetWind(startingWind,startingMagnitude);
        foreach (var ring in rings)
        {
            ring.SetActive(false);
        }
        rings[0].SetActive(true);
        WaveManager.instance.ChangeWaveValues(initialXAmp,initialXLenght,initialZAmp,initialZLenght);
    }

    public void UpdateRing(int index)
    {
        if (index >= rings.Count) return;
        if ((index + 1) >= rings.Count)
        {
            _finishedRings = true;
            GameManager.Instance.autoSailPositioning = false;
            UIManager.Instance.SetActiveSailControls(true);
            WindManager.instance.SetWind(new Vector2(0,-1),startingMagnitude+1);
            island.SetActive(true);
        }
        else
        {
            WaveManager.instance.ChangeWaveValues(initialXAmp+ampIncrease,initialXLenght,initialZAmp+ampIncrease,initialZLenght);
            rings[index+1].SetActive(true);
        }
    }
}
