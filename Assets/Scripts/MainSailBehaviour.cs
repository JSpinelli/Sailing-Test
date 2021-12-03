using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class MainSailBehaviour : MonoBehaviour
{
    public FloatReference rope;
    public FloatReference mainSailContribution;
    public StringReference pointOfSail;

    public BoolReference mainSailWorking;
    
    
    private float curvePoint;

    void Update()
    {
        UpdateContribution();
    }

    void UpdateContribution()
    {
        float mainSailMin = 0;
        float mainSailMax = 0;
        switch (pointOfSail.Value)
        {
            case "In Irons":
            {
                mainSailContribution.Value = Mathf.Lerp(mainSailContribution.Value, 0, Time.deltaTime);
                if (GameManager.Instance.autoSailPositioning) rope.Value = 5;
                break;
            }
            case "Close Hauled":
            {
                mainSailMax = 15;
                mainSailMin = 0;
                if (GameManager.Instance.autoSailPositioning) rope.Value = 10;
                break;
            }
            case "Close Reach":
            {
                mainSailMax = 25;
                mainSailMin = 10;
                if (GameManager.Instance.autoSailPositioning) rope.Value = 20;
                break;
            }
            case "Beam Reach":
            {
                mainSailMax = 35;
                mainSailMin = 20;
                if (GameManager.Instance.autoSailPositioning) rope.Value = 30;
                break;
            }
            case "Broad Reach":
            {
                mainSailMax = 45;
                mainSailMin = 30;
                if (GameManager.Instance.autoSailPositioning) rope.Value = 40;
                break;
            }
            case "Running":
            {
                mainSailMax = 55;
                mainSailMin = 40;
                if (GameManager.Instance.autoSailPositioning) rope.Value = 50;
                break;
            }
        }
        if (rope.Value < mainSailMax && rope > mainSailMin)
        {
            mainSailContribution.Value = Mathf.Lerp(mainSailContribution.Value, 1, Time.deltaTime);
            mainSailWorking.Value = true;
        }
        else
        {
            mainSailContribution.Value = Mathf.Lerp(mainSailContribution.Value, .5f, Time.deltaTime);
            mainSailWorking.Value = false;
        }
    }
}
