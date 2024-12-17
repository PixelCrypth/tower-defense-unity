using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr; // Reference to the SpriteRenderer component
    [SerializeField] private Color hoverColor; // Color when the mouse hovers over the plot
    public GameObject towerObj; // Reference to the tower built on this plot
    private Component turret; // Use Component to reference either Turret or ElectroTurret
    private Color startColor; // Initial color of the plot

    private void Start()
    {
        startColor = sr.color; // Store the initial color
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI is open over the plot, cannot interact.");
            return;
        }
        sr.color = hoverColor; // Change color on mouse hover
    }

    private void OnMouseExit()
    {
        sr.color = startColor; // Revert to initial color when mouse exits
    }

    public interface IUpgradeable
    {
        void OpenUpgradeUI();
    }

    private void OnMouseDown()
    {
        // Prevent building if the mouse is hovering over the UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI is open over the plot, cannot interact.");
            return;
        }

        // Prevent building if the mouse is hovering over the UI
        // Add your condition here to check if the mouse is over the UI
        // if (IsPointerOverUIObject()) return;
        // if (UIManager.Instance.IsUpgradeUIOpen()) return;

        if (towerObj != null)
        {
            if (turret != null)
            {
                // Check if the turret implements IUpgradeable and open the upgrade UI
                if (turret is IUpgradeable upgradeableTurret)
                {
                    upgradeableTurret.OpenUpgradeUI();
                }


                // Check the type of turret and open the corresponding upgrade UI
                // if (turret is Turret) // check if the turret = basic turret
                // {
                //     ((Turret)turret).OpenUpgradeUI();
                // }
                // else if (turret is ElectroTurret) // check if the turret = electro turret
                // {
                //     ((ElectroTurret)turret).OpenUpgradeUI();
                // }
            }


            else
            {
                Debug.LogError("Turret.cs or ElectroTurret.cs component is missing on the tower object!");
            }
            Debug.Log("Can't build there! - TODO: Display on screen"); // Prevent building if a tower is already present
            return;
        }

        Tower towerToBuild = BuildManager.main.GetSelectedTower(); // Get the selected tower to build

        if (towerToBuild.silver > LevelManager.main.silver)
        {
            Debug.Log("Not enough silver! - TODO: Display on screen"); // Prevent building if not enough silver
            return;
        }
        if (towerToBuild.gold > LevelManager.main.gold)
        {
            Debug.Log("Not enough gold! - TODO: Display on screen"); // Prevent building if not enough gold
            return;
        }

        LevelManager.main.SpendCurrencySilver(towerToBuild.silver); // Spend silver
        LevelManager.main.SpendCurrencyGold(towerToBuild.gold); // Spend gold

        Vector3 adjustedPosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z)); // Adjust position to grid
        towerObj = Instantiate(towerToBuild.prefab, adjustedPosition, Quaternion.identity); // Instantiate the tower

        // Try to get either a Turret or an ElectroTurret component
        turret = towerObj.GetComponent<Turret>() ?? (Component)towerObj.GetComponent<ElectroTurret>();

        if (turret == null)
        {
            Debug.LogError("Turret.cs or ElectroTurret.cs component is missing on the instantiated tower object!");
            Destroy(towerObj); // Destroy the instantiated object if Turret or ElectroTurret component is missing
            return;
        }
    }
}
