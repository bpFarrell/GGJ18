﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCRT : MonoBehaviour {
    public Material mat;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, mat);
    }
}
