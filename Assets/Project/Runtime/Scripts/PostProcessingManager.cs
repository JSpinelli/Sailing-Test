using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{
    public static PostProcessingManager Instance;
    public Volume volume;

    public float timeSwitchEffectDuration;

    public AnimationCurve lensDistortionTimeEffect;

    private bool timeSwitchActive;
    private float timeSwitchTimer;

    private FilmGrain _filmGrain;
    private LensDistortion _lensDistortion;
    private ChromaticAberration _chromaticAberration;

    public GameObject planeFilter;

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

    private void Start()
    {
       volume.profile.TryGet(out _filmGrain);
       volume.profile.TryGet(out _lensDistortion);
       volume.profile.TryGet(out _chromaticAberration);
    }

    public void EnableSeaFilter()
    {
        planeFilter.SetActive(true);
    }
    
    public void DisableSeaFilter()
    {
        planeFilter.SetActive(false);
    }

    private void Update()
    {
        if (timeSwitchActive)
        {
            if (timeSwitchTimer < timeSwitchEffectDuration)
            {
                timeSwitchTimer += Time.deltaTime;
                _lensDistortion.intensity.value = lensDistortionTimeEffect.Evaluate(timeSwitchTimer / timeSwitchEffectDuration);
            }
            else
            {
                Debug.Log("Turning Off Effect");
                _filmGrain.active = false;
                _lensDistortion.active = false;
                _chromaticAberration.active = false;
                timeSwitchActive = false;
            }
        }
    }

    public void TriggerTimeSwitchEffect()
    {
        timeSwitchActive = true;
        timeSwitchTimer = 0;
        _filmGrain.active = true;
        _lensDistortion.active = true;
        _chromaticAberration.active = true;
    }


}