using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject controls;

    public GameObject backButton;
    public GameObject resumeButton;
    public EventSystem eventSystem;


    private void Start()
    {
        startMenu.SetActive(true);
        controls.SetActive(false);
    }
    
    public void ShowControls()
    {
        startMenu.SetActive(false);
        controls.SetActive(true);
        eventSystem.SetSelectedGameObject(backButton);
    }

    public void HideControls()
    {
        startMenu.SetActive(true);
        controls.SetActive(false);
        eventSystem.SetSelectedGameObject(resumeButton);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
