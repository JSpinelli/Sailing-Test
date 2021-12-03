using System;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using Random = UnityEngine.Random;

public class SailSound : MonoBehaviour
{

    public AudioSource audioSource;

    public bool isMain = false;

    public float lerpFactor;
    public StringReference pointOfSailing;
    
    public BoolReference mainSailWorking;
    public BoolReference frontSailWorking;

    private void Start()
    {
        audioSource.PlayDelayed(Random.Range(0,1f));
    }

    void Update()
    {
        switch (pointOfSailing.Value)
        {
            case "In Irons":
            {
                audioSource.volume = Mathf.Lerp(audioSource.volume, 0.8f, Time.deltaTime * lerpFactor);
                break;
            }
            default:
            {
                if (isMain)
                {
                    if (mainSailWorking.Value)
                        audioSource.volume = Mathf.Lerp(audioSource.volume, 0.01f, Time.deltaTime * lerpFactor);
                    else
                        audioSource.volume = Mathf.Lerp(audioSource.volume, 0.8f, Time.deltaTime * lerpFactor);
                }
                if (!isMain)
                {
                    if (frontSailWorking.Value)
                        audioSource.volume = Mathf.Lerp(audioSource.volume, 0.01f, Time.deltaTime * lerpFactor);
                    else
                        audioSource.volume = Mathf.Lerp(audioSource.volume, 0.8f, Time.deltaTime);
                }
                break;
            }
        }
    }
}