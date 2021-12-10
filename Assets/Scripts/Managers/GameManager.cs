using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool autoMainSailPositioning = true;
    public bool autoFrontSailPositioning = true;
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
    
    // Update is called once per frame
    void Update()
    {
        if (PlayerController.paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
