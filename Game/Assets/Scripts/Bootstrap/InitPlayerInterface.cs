using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPlayerInterface : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
    }
}
