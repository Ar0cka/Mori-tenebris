using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsActiveController : MonoBehaviour
{
    [SerializeField] private GameObject itemPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            itemPanel.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            itemPanel.SetActive(false);
        }
    }

    public void ControlItemPanel()
    {
        bool active = !itemPanel.activeSelf ? true : false;
        itemPanel.SetActive(active);
    }
}
