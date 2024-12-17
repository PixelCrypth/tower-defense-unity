using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    // https://youtu.be/y2N_J391ptg?si=I4p9QJWAaAJ7zpu8
    private static TooltipManager _instance;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI tooltipTitle;
    [SerializeField] private TextMeshProUGUI[] tooltipLines;
    [SerializeField] private Image tooltipImage;

    public static TooltipManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("TooltipManager is NULL");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);

        tooltipTitle.text = "Test Title";
        tooltipLines[0].text = "This is the first line of the tooltip.";
        tooltipLines[1].text = "This is the second line of the tooltip.";
        tooltipLines[2].text = "This is the third line of the tooltip.";

        // Load the sprite from the Resources folder
        Sprite testImage = Resources.Load<Sprite>("Art/Sprites/Tower2_thumbnail");

        if (testImage != null)
        {
            tooltipImage.sprite = testImage;
        }
        else
        {
            Debug.LogWarning("Sprite not found at specified path.");
        }
    }

    // void Update()
    // {
    //     Vector3 mousePos = Input.mousePosition;
    //     mousePos.x += 100; // Adjust the x position to be 100 pixels to the right of the mouse
    //     mousePos.y -= 150; // Adjust the y position to be 150 pixels below the mouse

    //     // Check if the tooltip goes out of the screen bounds
    //     if (mousePos.x + tooltipImage.rectTransform.rect.width > Screen.width)
    //     {
    //         mousePos.x -= 200; // Move to the left if it goes out of the right screen edge
    //     }
    //     if (mousePos.y - tooltipImage.rectTransform.rect.height < 0)
    //     {
    //         mousePos.y += 280; // Move up if it goes out of the bottom screen edge
    //     }

    //     transform.position = mousePos;
    // }

void Update()
{
    // Get the mouse position in screen space
    Vector3 mousePos = Input.mousePosition;

    // Convert the screen position to a position relative to the canvas
    Canvas canvas = GetComponentInParent<Canvas>();
    RectTransform canvasRect = canvas.transform as RectTransform;

    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
        canvasRect, 
        mousePos, 
        canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera, 
        out Vector2 localPoint))
    {
        // Offset the tooltip slightly below and to the right of the mouse
        localPoint.x += 100; // Horizontal offset
        localPoint.y -= 260; // Vertical offset

        // Assign the position to the tooltip's RectTransform
        RectTransform tooltipRect = GetComponent<RectTransform>();
        tooltipRect.localPosition = localPoint;

        // Clamp the tooltip to prevent it from going outside the screen
        ClampTooltipToScreenBounds(canvasRect, tooltipRect);
    }
}

void ClampTooltipToScreenBounds(RectTransform canvasRect, RectTransform tooltipRect)
{
    Vector3 pos = tooltipRect.localPosition;
    Vector2 size = tooltipRect.sizeDelta;

    // Clamp X-axis (left and right bounds)
    float halfWidth = size.x / 2;
    pos.x = Mathf.Clamp(pos.x, -canvasRect.rect.width / 2 + halfWidth, canvasRect.rect.width / 2 - halfWidth);

    // Clamp Y-axis (top and bottom bounds)
    float halfHeight = size.y / 2;
    pos.y = Mathf.Clamp(pos.y, -canvasRect.rect.height / 2 + halfHeight, canvasRect.rect.height / 2 - halfHeight);

    tooltipRect.localPosition = pos;
}




    public void SetAnShowToolTip(string title, string[] lines, Sprite image)
    {
        gameObject.SetActive(true);
        tooltipTitle.text = title;
        Debug.Log(tooltipLines.Length);
        for (int i = 0; i < tooltipLines.Length; i++)
        {
            if (i < lines.Length)
            {
                tooltipLines[i].text = lines[i];
            }
            else
            {
                tooltipLines[i].text = string.Empty;
            }
        }

        tooltipImage.sprite = image;
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
        tooltipTitle.text = string.Empty;
        tooltipImage.sprite = null;

        foreach (var line in tooltipLines)
        {
            line.text = string.Empty;
        }
    }
}
