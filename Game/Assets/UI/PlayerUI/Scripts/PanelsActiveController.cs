using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsActiveController : MonoBehaviour
{
    [SerializeField] private GameObject itemPanel;

    public void ControlItemPanel()
    {
        bool active = !itemPanel.activeSelf ? true : false;
        itemPanel.SetActive(active);
    }
}
