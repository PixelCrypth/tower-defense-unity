using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencySilverUI; // Reference to the money text
    [SerializeField] TextMeshProUGUI currencyGoldUI; // Reference to the money text
    [SerializeField] Animator anim;

    private bool isMenuOpen = true;

    public bool GetMenuState() // so others can get info if menu is open or not
    {
        return isMenuOpen;
    }

    public void ToggleMenu() {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("MenuOpen", isMenuOpen);
    }

    private void OnGUI()
    {
        currencySilverUI.text = "Silver: " + LevelManager.main.silver.ToString(); // Update the text to display the current amount of silver from the LevelManager
        currencyGoldUI.text = "Gold: " + LevelManager.main.gold.ToString(); // Update the text to display the current amount of silver from the LevelManager
    }


}
