﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class CameraOrthoScale : MonoBehaviour {

    public float horizontalResolution = 1024;

    void OnGUI()
    {
        float currentAspect = (float)Screen.width / (float)Screen.height;
        GetComponent<Camera>().orthographicSize = horizontalResolution / currentAspect / 200;
    }
}
