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

    public Texture[] Sailing;

    public RawImage display;

    public StringReference pointOfSailing;
    public IntReference speedVal;
    public TextMeshProUGUI typeOfSailing;
    public TextMeshProUGUI speed;

    public FloatReference mainSailRope;
    public FloatReference frontSailRope;
    public TextMeshProUGUI mainSailRopeUI;
    public TextMeshProUGUI frontSailRopeUI;
    void Update()
    {
        typeOfSailing.text = pointOfSailing.Value;
        speed.text = "Speed: "+speedVal.Value;
        frontSailRopeUI.text = "Front Sail Rope: " + (int) (frontSailRope);
        mainSailRopeUI.text = "Main Sail Rope: " + (int) (mainSailRope);
        switch (pointOfSailing.Value)
        {
            case "In Irons":
            {
                display.texture = Sailing[5];
                break;
            }
            case "Close Hauled":
            {
                if (bm.mainSailWorking && bm.frontSailWorking)
                {
                    display.texture = correctSailing[0];
                }
                else
                {
                    display.texture = Sailing[0];
                }
                break;
            }
            case "Close Reach":
            {
                if (bm.mainSailWorking && bm.frontSailWorking)
                {
                    display.texture = correctSailing[1];
                }
                else
                {
                    display.texture = Sailing[1];
                }
                break;
            }
            case "Beam Reach":
            {
                if (bm.mainSailWorking && bm.frontSailWorking)
                {
                    display.texture = correctSailing[2];
                }
                else
                {
                    display.texture = Sailing[2];
                }
                break;
            }
            case "Broad Reach":
            {
                if (bm.mainSailWorking && bm.frontSailWorking)
                {
                    display.texture = correctSailing[3];
                }
                else
                {
                    display.texture = Sailing[3];
                }
                break;
            }
            case "Running":
            {
                if (bm.mainSailWorking && bm.frontSailWorking)
                {
                    display.texture = correctSailing[4];
                }
                else
                {
                    display.texture = Sailing[4];
                }
                break;
            }
        }
    }
}
