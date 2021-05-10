using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PostProcessing : MonoBehaviour
{
    public Material[] postProcessingMats;
    public Camera cam;
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        for (int i = 0; i < postProcessingMats.Length -1 ; i++)
        {
            Graphics.Blit(src,src,postProcessingMats[i]);
        }
        Graphics.Blit(src,dest,postProcessingMats[postProcessingMats.Length-1]);
    }
}
