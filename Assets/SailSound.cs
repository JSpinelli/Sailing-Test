using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SailSound : MonoBehaviour
{
    public BoatManager bm;

    public AudioSource audioSource;

    public bool isMain = false;

    public float lerpFactor;

    private void Start()
    {
        audioSource.PlayDelayed(Random.Range(0,1f));
    }

    void Update()
    {
        switch (bm.typeOfSailing.text)
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
                    if (bm.mainSailWorking)
                        audioSource.volume = Mathf.Lerp(audioSource.volume, 0.01f, Time.deltaTime * lerpFactor);
                    else
                        audioSource.volume = Mathf.Lerp(audioSource.volume, 0.8f, Time.deltaTime * lerpFactor);
                }
                if (!isMain)
                {
                    if (bm.frontSailWorking)
                        audioSource.volume = Mathf.Lerp(audioSource.volume, 0.01f, Time.deltaTime * lerpFactor);
                    else
                        audioSource.volume = Mathf.Lerp(audioSource.volume, 0.8f, Time.deltaTime);
                }
                break;
            }
        }
    }
}