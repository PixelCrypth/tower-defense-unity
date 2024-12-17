using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
public class ElectroTurret : MonoBehaviour, Plot.IUpgradeable
{
    [Header("Debug")]
    [SerializeField] private bool debugMode = false; // Debug mode to display logs

    [Header("References")]
    [SerializeField] private Transform turretRotationPoint; // The point around which the turret rotates
    [SerializeField] private LayerMask enemyMask; // The layer mask to identify enemies
    [SerializeField] private GameObject bulletPrefab; // Bullet prefab to instantiate
    [SerializeField] private Transform firingPoint; // The point from which the bullet is spawned


    [Header("Upgrade/Sell UI (GameObjects)")]
    [SerializeField] private GameObject upgradeAndSellUI; // UI for the turret
    [SerializeField] private Button upgradeButton; // upgrade button
    [SerializeField] private GameObject turretCostText; // text of turret cost to upgrade
    [SerializeField] private GameObject bulletSpeedText; // 
    [SerializeField] private GameObject bulletDamageText; // 
    [SerializeField] private GameObject targetingRangeText; // 
    [SerializeField] private GameObject bpsText; // 

    [SerializeField] private Button sellButton; // sell button

    [Header("Attributes")]
    [SerializeField] private float stunDuration = 2f; // Duration of the stun effect
    [SerializeField] private Color stunColor = Color.yellow; // Color to flash when stunned
    [SerializeField] private float bulletSpeed = 15f; // Speed of the bullet
    [SerializeField] private float bulletDamage = 1f; // Damage dealt by the bullet
    [SerializeField] private float targetingRange = 5f; // The range within which the turret can target enemies
    [SerializeField] private Vector3 circleOffset = Vector3.zero; // Offset for the targeting circle
    [SerializeField] private float rotationSpeed = 5f; // The speed at which the turret rotates
    [SerializeField] private float bps = 1f; // Bullet per second
    [SerializeField] private int upgradeCost = 50; // The cost to upgrade the turret

    private Transform target; // The current target of the turret
    private float timeUntilFire;
    private float bpsBase;
    private float targetingRangeBase;
    private int level = 1; // The current level of the turret

    private void Start()
    {
        bpsBase = bps;
        targetingRangeBase = targetingRange;

        upgradeButton.onClick.AddListener(UpgradeTurret);
        sellButton.onClick.AddListener(SellTurret);
        UpdateUpgradeCostText();
    }

    private void Update()
    {
        // If there is no target or the target is out of range, find a new target
        if (target == null || !CheckTargetIsInRange())
        {
            FindTarget();
        }

        // If a target is found, rotate towards it
        if (target != null && CheckTargetIsInRange())
        {
            RotateTowardsTarget();
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
        else
        {
            if (!CheckTargetIsInRange())
            {
                target = null;
                // Stop rotating if no target is found or target is out of range
                StopRotating();
            }
        }

    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Electro_Bullet bulletScript = bulletObj.GetComponent<Electro_Bullet>(); // change this to the bullet script or objectname you are using
        bulletScript.SetTarget(target);
        bulletScript.SetDamage(bulletDamage); // send bulletdmg over to the bullet script
        bulletScript.SetSpeed(bulletSpeed); // send bulletspeed over to the bullet script
        bulletScript.SetRange(targetingRange); // send targetingrange over to the bullet script
        bulletScript.SetStunDuration(stunDuration); // send stun duration over to the bullet script
        bulletScript.SetStunColor(stunColor); // send stun color over to the bullet script

        DebugLog("Shot fired at target: " + (target != null ? target.name : "None"));

    }

    private void FindTarget()
    {
        // Perform a circle cast to find all enemies within the targeting range
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position + circleOffset, targetingRange, Vector2.zero, 0f, enemyMask);

        // If any enemies are found, set the first one as the target
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }


    }



    private bool CheckTargetIsInRange()
    {
        if (target == null) return false;
        bool inRange = Vector2.Distance(target.position, transform.position) <= targetingRange;

        // DebugLog("CheckTargetIsInRange called. Target in range: " + inRange);

        return inRange;
    }

    private void RotateTowardsTarget()
    {
        // Calculate the angle to the target
        float angle = Mathf.Atan2(target.position.y - turretRotationPoint.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        // Create a rotation based on the calculated angle
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        // Apply the rotation to the turret rotation point
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // DebugLog("RotateTowardsTarget called. Target angle: " + angle);
    }

    private void StopRotating()
    {
        // Optionally, you can reset the rotation or keep it as is
        // Rotate back to the original position using the rotation speed
        Quaternion originalRotation = Quaternion.identity;
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, originalRotation, rotationSpeed * Time.deltaTime);

        DebugLog("StopRotating called.");
    }

    // UPGRADE TURRET AND SELL TURRET START HERE //
    private void UpdateUpgradeCostText()
    {
        TextMeshProUGUI costText = turretCostText.GetComponent<TextMeshProUGUI>();
        costText.text = "Upgrade Cost: " + CalculateUpgradeCost().ToString() + " Silver";
        TextMeshProUGUI speedText = bulletSpeedText.GetComponent<TextMeshProUGUI>();
        speedText.text = "Bullet Speed: " + bulletSpeed.ToString() + " units/s";

        TextMeshProUGUI damageText = bulletDamageText.GetComponent<TextMeshProUGUI>();
        damageText.text = "Bullet Damage: " + bulletDamage.ToString();

        TextMeshProUGUI rangeText = targetingRangeText.GetComponent<TextMeshProUGUI>();
        rangeText.text = "Targeting Range: " + targetingRange.ToString("F1") + " units";

        TextMeshProUGUI bpsTextComponent = bpsText.GetComponent<TextMeshProUGUI>();
        bpsTextComponent.text = "Bullet Per Second: " + bps.ToString("F1");
    }

    public void OpenUpgradeUI()
    {
        DebugLog("Opening upgrade UI for turret: " + gameObject.name);
        upgradeAndSellUI.SetActive(true);
        UIManager.main.SetHoveringState(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeAndSellUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    public void UpgradeTurret()
    {
        if (CalculateUpgradeCost() > LevelManager.main.silver) // Check if you got enough silver to upgrade the turret
        {
            DebugLog("Not enough silver to upgrade the turret!"); // 
            CloseUpgradeUI();
            return;
        }
        LevelManager.main.SpendCurrencySilver(CalculateUpgradeCost()); // Spend the silver to upgrade the turret
        level++; // Increase the level of the turret
        bps = CalculateBPS(); // Calculate the new BPS of the turret
        targetingRange = CalculateRange(); // Calculate the new range of the turret
        bulletDamage = bulletDamage + 1f; // Calculate the new bullet damage
        bulletSpeed = bulletSpeed + 0.1f; // Calculate the new bullet speed
        stunDuration = stunDuration + 1f; // Calculate the new stun duration
        // Upgrade the turret
        DebugLog("Turret upgraded!");
        DebugLog("New BPS: " + bps);
        DebugLog("New Range: " + targetingRange);
        DebugLog("New Damage: " + bulletDamage);
        DebugLog("New Speed: " + bulletSpeed);
        DebugLog("New Stun Duration: " + stunDuration);
        DebugLog("Upgrade cost: " + CalculateUpgradeCost());
        CloseUpgradeUI();
        UpdateUpgradeCostText();
    }

    public void SellTurret()
    {
        // Shorten it to just the turret name
        string turretName = gameObject.name.Replace("(Clone)", "");

        // Get the turret data from the BuildManager.cs script
        int turretPrice = BuildManager.main.GetTurretDataByName(turretName).gold;

        if (turretPrice <= 0)
        {
            DebugLog("Turret data not found or invalid price!");
            DebugLog("Are you sure you named the Prefab the same as the shop item?");
            DebugLog("Did you try to sell a Turret for 0 cost? This is not allowed.");

            return;
        }

        // // Refund the silver price of the turret (50% of total rpice)
        LevelManager.main.IncreaseCurrencyGold(turretPrice / 2);

        CloseUpgradeUI();
        Destroy(gameObject); // Destroy the turret
    }

    private int CalculateUpgradeCost()
    {
        // This formula calculates the upgrade cost for the turret.
        // It multiplies the base upgrade cost by the level of the turret raised to the power of 0.8,
        // and then rounds the result to the nearest integer.
        // Mathf.RoundToInt: Rounds the floating-point result to the nearest integer.
        // upgradeCost: The base cost for upgrading the turret.
        // Mathf.Pow(level, 0.8f): Raises the turret's level to the power of 0.8 to create a non-linear scaling effect.
        DebugLog("Upgrade cost: " + Mathf.RoundToInt(upgradeCost * Mathf.Pow(level, 0.8f)));
        return Mathf.RoundToInt(upgradeCost * Mathf.Pow(level, 0.8f)); // Change this formula to suit your needs for upgrading the turret
    }

    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.4f); // Change this formula to suit your needs for upgrading the range of the turret
    }

    private float CalculateBPS()
    {
        return bpsBase * Mathf.Pow(level, 0.5f); // Change this formula to suit your needs for upgrading the BPS of the turret
    }

    // UPGRADE TURRET AND SELL TURRET STOPS HERE //
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            // Set the color for the gizmo
            Handles.color = Color.cyan;
    
            // Draw a wire disc to represent the targeting range
            Vector3 adjustedOffset = circleOffset;
            Handles.DrawWireDisc(transform.position + adjustedOffset, transform.forward, targetingRange);
        }
#endif

    private void DebugLog(string message)
    {
        if (debugMode)
        {
            Debug.Log(message);
        }
    }
}
