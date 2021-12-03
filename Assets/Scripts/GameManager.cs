using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public BoatManager boatManager;

    public bool startTutorial = true;

    public bool autoSailPositioning = true;
    private bool _tillerTutorial = true;
    private bool _windTutorial = false;
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

    // Start is called before the first frame update
    void Start()
    {
        if (startTutorial)
        {
            autoSailPositioning = true;
            _tillerTutorial = true;
            _windTutorial = false;
            UIManager.Instance.SetActiveSailControls(false);
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
