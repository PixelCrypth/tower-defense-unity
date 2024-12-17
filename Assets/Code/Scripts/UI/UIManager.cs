using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager main;
    public static UIManager Instance { get; private set; }

    private bool isHoveringUI;
    private bool isShopOpen;
    public GameObject upgradeUI;

    private void Awake()
    {
        main = this;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool IsUpgradeUIOpen()
    {
        return upgradeUI.activeSelf;
    }
    public void SetHoveringState(bool state)
    {
        Debug.Log("Hovering UI: " + state);
        isHoveringUI = state;
    }

    public bool GetHoveringState()
    {
        Debug.Log("Hovering UI: " + isHoveringUI);
        return isHoveringUI;
    }

    public bool IsHoveringUI()
    {
        Debug.Log("Hovering UI: " + isHoveringUI);
        return isHoveringUI;
    }


}
