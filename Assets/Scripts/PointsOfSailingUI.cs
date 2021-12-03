using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

public class PointsOfSailingUI : MonoBehaviour
{
    public BoatManager bm;
    
    public Texture[] correctSailing;
    
    public Texture[] correctSailingInverted;

    public Texture[] Sailing;
    
    public Texture[] SailingInverted;

    public RawImage display;

    public StringReference pointOfSailing;
    public IntReference speedVal;
    public TextMeshProUGUI typeOfSailing;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI leftStickText;

    public FloatReference mainSailRope;
    public FloatReference frontSailRope;
    public TextMeshProUGUI mainSailRopeUI;
    public TextMeshProUGUI frontSailRopeUI;
    public BoolReference mainSailWorking;
    public BoolReference frontSailWorking;

    private Texture[] mySailingNow;
    private Texture[] mySailingCorrectNow;
    
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
            mySailingNow = Sailing;
            mySailingCorrectNow = correctSailing;
        }
        else
        {
            mySailingNow = SailingInverted;
            mySailingCorrectNow = correctSailingInverted;
        }

        switch (pointOfSailing.Value)
        {
            case "In Irons":
            {
                display.texture = mySailingNow[5];
                break;
            }
            case "Close Hauled":
            {
                if (mainSailWorking.Value && frontSailWorking.Value)
                {
                    display.texture = mySailingCorrectNow[0];
                }
                else
                {
                    display.texture = mySailingNow[0];
                }
                break;
            }
            case "Close Reach":
            {
                if (mainSailWorking.Value && frontSailWorking.Value)
                {
                    display.texture = mySailingCorrectNow[1];
                }
                else
                {
                    display.texture = mySailingNow[1];
                }
                break;
            }
            case "Beam Reach":
            {
                if (mainSailWorking.Value && frontSailWorking.Value)
                {
                    display.texture = mySailingCorrectNow[2];
                }
                else
                {
                    display.texture = mySailingNow[2];
                }
                break;
            }
            case "Broad Reach":
            {
                if (mainSailWorking.Value && frontSailWorking.Value)
                {
                    display.texture = mySailingCorrectNow[3];
                }
                else
                {
                    display.texture = mySailingNow[3];
                }
                break;
            }
            case "Running":
            {
                if (mainSailWorking.Value && frontSailWorking.Value)
                {
                    display.texture = mySailingCorrectNow[4];
                }
                else
                {
                    display.texture = mySailingNow[4];
                }
                break;
            }
        }
    }
}
