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
    public GameObject island;
    public Vector2 startingWind;
    public float startingMagnitude;
    public float tutorialTimer;
    public float initialXLenght;
    public float initialZLenght;
    public float initialXAmp;
    public float initialZAmp;

    public float ampIncrease;

    public TextMeshProUGUI text;

    public StringReference pointOfSailing;

    public bool startTutorial = true;

    public GameObject normalUI;
    public GameObject boat;
    public bool skipIntro = false;
    public GameObject introCard;
    public GameObject bg;
    public GameObject firstLine;
    public GameObject secondLine;
    public GameObject thirdLine;
    private bool _introFinished;

    public float timeBetweenLines = 4f;
    private float _timerForLines = 4f;
    private int _indexForLines = 1;


    private float _tutorialCounter;

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
        if (skipIntro)
        {
            normalUI.SetActive(true);
            _introFinished = true;
            introCard.SetActive(false);
            boat.SetActive(true);
            text.gameObject.SetActive(false);
            StartTutorial();
        }
        else
        {
            _timerForLines = 0;
            normalUI.SetActive(false);
            boat.SetActive(false);
            introCard.SetActive(true);
            _introFinished = false;
        }
    }

    private void IntroTimers()
    {
        if (_timerForLines < timeBetweenLines)
        {
            _timerForLines += Time.deltaTime;
        }
        else
        {
            _timerForLines = 0;
            _indexForLines++;
            if (_indexForLines == 2)
            {
                firstLine.SetActive(false);
                secondLine.SetActive(true);
            }
            else if (_indexForLines == 3)
            {
                secondLine.SetActive(false);
                thirdLine.SetActive(true);
                boat.SetActive(true);
            }
            else
            {
                _introFinished = true;
                introCard.SetActive(false);
                normalUI.SetActive(true);
                StartTutorial();
            }
        }
    }

    private void StartTutorial()
    {
        _tutorialCounter = 0;
        if (startTutorial)
        {
            GameManager.Instance.autoFrontSailPositioning = true;
            GameManager.Instance.autoMainSailPositioning = true;
            UIManager.Instance.SetActiveFrontSailControls(false);
            UIManager.Instance.SetActiveMainSailControls(false);
        }

        island.SetActive(false);
        WindManager.instance.SetWind(startingWind, startingMagnitude);
        foreach (var ring in rings)
        {
            ring.SetActive(false);
        }

        rings[0].SetActive(true);
        WaveManager.instance.ChangeWaveValues(initialXAmp, initialXLenght, initialZAmp, initialZLenght);
        UIManager.Instance.PointOfSailingViz(false);
    }

    private void Update()
    {
        if (_introFinished)
        {
            if (pointOfSailing.Value == "In Irons")
            {
                if (_tutorialCounter < tutorialTimer)
                {
                    _tutorialCounter += Time.deltaTime;
                }
                else
                {
                    text.gameObject.SetActive(true);
                    text.text = " I can't sail towards the wind, I need to adjust my position ";
                }
            }
            else
            {
                _tutorialCounter = 0;
                text.gameObject.SetActive(false);
                text.text = "";
            }
        }
        else
        {
            IntroTimers();
        }
    }

    public void UpdateRing(int index)
    {
        if (index >= rings.Count) return;
        if ((index) == 2)
        {
            GameManager.Instance.autoMainSailPositioning = false;
            UIManager.Instance.SetActiveMainSailControls(true);
            UIManager.Instance.PointOfSailingViz(true);
        }

        if ((index + 1) >= rings.Count)
        {
            GameManager.Instance.autoMainSailPositioning = false;
            UIManager.Instance.SetActiveMainSailControls(true);
            GameManager.Instance.autoFrontSailPositioning = false;
            UIManager.Instance.SetActiveFrontSailControls(true);
            UIManager.Instance.PointOfSailingViz(true);
            WindManager.instance.SetWind(new Vector2(0.2f, -0.1f).normalized, startingMagnitude + 1);
            island.SetActive(true);
        }
        else
        {
            WaveManager.instance.ChangeWaveValues(initialXAmp + ampIncrease, initialXLenght, initialZAmp + ampIncrease,
                initialZLenght);
            rings[index + 1].SetActive(true);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TutorialIslandManager))]
public class DrawTutorialIslandManager : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TutorialIslandManager manager = (TutorialIslandManager) target;
        if (GUILayout.Button("Finish Tutorial"))
        {
            manager.UpdateRing(manager.rings.Count - 1);
        }
    }
}
#endif