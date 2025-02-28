﻿using System;
using UnityEngine;
using System.Collections.Generic;

public class WebCamInput : MonoBehaviour
{
    [SerializeField] string webCamName;
    [SerializeField] Vector2 webCamResolution = new Vector2(1920, 1080);
    [SerializeField] Texture staticInput;

    // Provide input image Texture.
    public Texture inputImageTexture{
        get{
            if(staticInput != null) return staticInput;
            return inputRT;
        }
    }

    WebCamTexture webCamTexture;
    RenderTexture inputRT;

    void Start()
    {
        if (inputRT != null || webCamTexture != null) {
            try{
                resetCamera();
            }finally{}
        }
        if(staticInput == null){
            webCamTexture = new WebCamTexture(webCamName, (int)webCamResolution.x, (int)webCamResolution.y);
        }
        webCamTexture.Play();
        inputRT = new RenderTexture((int)webCamResolution.x, (int)webCamResolution.y, 0);
        
    }

    void Update()
    {
        // if (!webCamTexture.isPlaying) webCamTexture.Play();
        try{
            if(staticInput != null) return;
            if (webCamTexture != null)if(!webCamTexture.didUpdateThisFrame) return;
            var aspect1 = (float)webCamTexture.width / webCamTexture.height;
            var aspect2 = (float)inputRT.width / inputRT.height;
            var aspectGap = aspect2 / aspect1;

            var vMirrored = webCamTexture.videoVerticallyMirrored;
            // webCamTexture.videoHorizontallyMirrored = 1;
            var scale = new Vector2(aspectGap, vMirrored ? -1 : 1);
            var offset = new Vector2((1 - aspectGap) / 2, vMirrored ? 1 : 0);
            Graphics.Blit(webCamTexture, inputRT, scale, offset);
        }catch{

        }
        
    }

    public void resetCamera(){
        if (webCamTexture != null) webCamTexture.Stop();
    }

    void OnDestroy(){
        if (webCamTexture != null) Destroy(webCamTexture);
        if (inputRT != null) Destroy(inputRT);
    }
}
