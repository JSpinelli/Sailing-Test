using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    private Texture[] _mySailingNow;
    private Texture[] _mySailingCorrectNow;
    
    public BoatManager bm;
    public RawImage display;
    public Texture[] correctSailing;
    public Texture[] correctSailingInverted;
    public Texture[] Sailing;
    public Texture[] SailingInverted;
    
    public TextMeshProUGUI typeOfSailing;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI leftStickText;
    public GameObject mainSailControls;
    public GameObject frontSailControls;
    
    public IntReference speedVal;
    public FloatReference mainSailRope;
    public FloatReference frontSailRope;
    public TextMeshProUGUI mainSailRopeUI;
    public TextMeshProUGUI frontSailRopeUI;
    public BoolReference mainSailWorking;
    public BoolReference frontSailWorking;
    public StringReference pointOfSailing;

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

    void Update()
    {
        typeOfSailing.text = pointOfSailing.Value;
        speed.text = "Speed: "+speedVal.Value;
        frontSailRopeUI.text = "Front Sail Rope: " + (int) (frontSailRope);
        mainSailRopeUI.text = "Main Sail Rope: " + (int) (mainSailRope);
        if (PlayerController.tillerGrabbed)
        {
            leftStickText.text = "Move Tiller";
        }
        else
        {
            leftStickText.text = "Move Character";
        }

        if (bm.dot2 < 0)
        {
            _mySailingNow = Sailing;
            _mySailingCorrectNow = correctSailing;
        }
        else
        {
            _mySailingNow = SailingInverted;
            _mySailingCorrectNow = correctSailingInverted;
        }

        switch (pointOfSailing.Value)
        {
            case "In Irons":
            {
                display.texture = _mySailingNow[5];
                break;
            }
            case "Close Hauled":
            {
                if (mainSailWorking.Value && frontSailWorking.Value)
                {
                    display.texture = _mySailingCorrectNow[0];
                }
                else
                {
                    display.texture = _mySailingNow[0];
                }
                break;
            }
            case "Close Reach":
            {
                if (mainSailWorking.Value && frontSailWorking.Value)
                {
                    display.texture = _mySailingCorrectNow[1];
                }
                else
                {
                    display.texture = _mySailingNow[1];
                }
                break;
            }
            case "Beam Reach":
            {
                if (mainSailWorking.Value && frontSailWorking.Value)
                {
                    display.texture = _mySailingCorrectNow[2];
                }
                else
                {
                    display.texture = _mySailingNow[2];
                }
                break;
            }
            case "Broad Reach":
            {
                if (mainSailWorking.Value && frontSailWorking.Value)
                {
                    display.texture = _mySailingCorrectNow[3];
                }
                else
                {
                    display.texture = _mySailingNow[3];
                }
                break;
            }
            case "Running":
            {
                if (mainSailWorking.Value && frontSailWorking.Value)
                {
                    display.texture = _mySailingCorrectNow[4];
                }
                else
                {
                    display.texture = _mySailingNow[4];
                }
                break;
            }
        }
    }
    
    public void SetActiveSailControls(bool active)
    {
        mainSailControls.SetActive(active);
        frontSailControls.SetActive(active);
    }
}
