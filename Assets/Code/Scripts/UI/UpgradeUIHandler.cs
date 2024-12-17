using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // ignore warning for mouse_over not being used in this script as it is used in the UIManager script
    #pragma warning disable CS0414
    private bool mouse_over = false;
    #pragma warning disable CS0414

    [SerializeField] private bool DebugMode = false;
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        UIManager.main.SetHoveringState(true);
        
        if (DebugMode)
        {
        Debug.Log("Pointer entered");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        UIManager.main.SetHoveringState(false);
        gameObject.SetActive(false);
        if (DebugMode)
        {
        Debug.Log("Pointer exited");
        }
    }
}
