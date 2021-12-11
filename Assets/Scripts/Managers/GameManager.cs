using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool autoMainSailPositioning = true;
    public bool autoFrontSailPositioning = true;
    public bool gamePaused = false;
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
        // if (PlayerController.paused)
        //     Pause();
    }

    public void Pause()
    {
        gamePaused = !gamePaused;
        if (gamePaused)        
        {
            UIManager.Instance.ShowPauseMenu();
            Time.timeScale = 0;
        }
        else
        {
            UIManager.Instance.HidePauseMenu();
            Time.timeScale = 1;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
