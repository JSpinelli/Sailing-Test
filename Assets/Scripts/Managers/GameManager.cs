using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void Pause()
    {
        gamePaused = !gamePaused;
        if (gamePaused)        
        {
            UIManager.Instance.ShowPauseMenu();
            PlayerController.Instance.EnableUIInput();
            Time.timeScale = 0;
        }
        else
        {
            UIManager.Instance.HidePauseMenu();
            PlayerController.Instance.EnableBoatInput();
            Time.timeScale = 1;
        }
    }

    public void ReloadScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Tutorial Island");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
