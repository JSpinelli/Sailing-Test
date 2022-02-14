using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public GameObject startMenu;
    public GameObject controls;

    public GameObject backButton;
    public GameObject startButton;
    public EventSystem eventSystem;


    private void Start()
    {
        startMenu.SetActive(true);
        controls.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Tutorial Island");
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
        eventSystem.SetSelectedGameObject(startButton);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
