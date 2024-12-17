using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] public float hitPoints = 2f; // Initial hit points
    [SerializeField] public int silverWorth = 50; // Currency value when destroyed

    private bool isDestroyed = false; // Flag to check if already destroyed (prevent bugs like being double destroyed)

    // Method to apply damage
    public void TakeDamage(float damage)
    {
        hitPoints -= damage; // Reduce hit points
        if (hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke(); // Trigger enemy destroy event
            LevelManager.main.IncreaseCurrencySilver(silverWorth); // Increase player's currency
            isDestroyed = true; // Mark as destroyed
            Destroy(gameObject); // Destroy the game object
        }
    }
}
