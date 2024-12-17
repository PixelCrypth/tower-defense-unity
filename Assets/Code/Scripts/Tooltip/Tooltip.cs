using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private string tooltipTitle; // Reference to the tooltip object
    [SerializeField] private string[] tooltipLines; // Reference to the tooltip message
    [SerializeField] private Sprite tooltipImage; // Reference to the tooltip image

    private bool isUIElement;

    private void Start()
    {
        isUIElement = GetComponent<Graphic>() != null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isUIElement)
        {
            Debug.Log("Mouse entered UI element");
            TooltipManager.Instance.SetAnShowToolTip(tooltipTitle, tooltipLines, tooltipImage); // Show the tooltip when the mouse enters the object
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isUIElement)
        {
            TooltipManager.Instance.HideToolTip(); // Hide the tooltip when the mouse exits the object
        }
    }

    private void OnMouseEnter()
    {
        if (!isUIElement)
        {
            Debug.Log("Mouse entered game object");
            TooltipManager.Instance.SetAnShowToolTip(tooltipTitle, tooltipLines, tooltipImage); // Show the tooltip when the mouse enters the object
        }
    }

    private void OnMouseExit()
    {
        if (!isUIElement)
        {
            TooltipManager.Instance.HideToolTip(); // Hide the tooltip when the mouse exits the object
        }
    }
}
