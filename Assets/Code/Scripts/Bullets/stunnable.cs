using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stunnable : MonoBehaviour
{
    [Header("References")] // Correct attribute name
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private EnemyMovement enemyMovement;
    [Header("Attributes")] // Correct attribute name
    [SerializeField] private bool isStunned = false; // Tracks if the enemy is currently stunned
    [SerializeField] private bool debugMode = false; // Duration of the stun effect

    public void Stun(float stunDuration, Color stunColor)
    {
        if (!isStunned)
        {
            Debug.Log(stunColor);

            StartCoroutine(StunCoroutine(stunDuration, stunColor));
        }
        else
        {
            if (debugMode) {
                Debug.Log("Enemy is already stunned.");
            }
        }
    }

    // Coroutine to handle the stun effect
    private IEnumerator StunCoroutine(float stunDuration, Color stunColor)
    {
        isStunned = true; // Mark the enemy as stunned

        if (spriteRenderer != null && enemyMovement != null)
        {
            Color originalColor = spriteRenderer.color; // Capture the original color
            float originalSpeed = enemyMovement.GetSpeed(); // Capture the original speed

            // Apply stun effect
            enemyMovement.UpdateSpeed(0f); // Set speed to 0 to stun the enemy
            if (debugMode) {
                Debug.Log("Stun applied for " + stunDuration + " seconds.");
            }

            float blinkInterval = 0.2f; // Interval for blinking
            float elapsedTime = 0f;

            while (elapsedTime < stunDuration)
            {
                spriteRenderer.color = stunColor; // Change to stun color
                yield return new WaitForSeconds(blinkInterval);
                spriteRenderer.color = originalColor; // Reset to original color
                yield return new WaitForSeconds(blinkInterval);
                elapsedTime += 2 * blinkInterval;
            }

            // Restore the original state
            enemyMovement.UpdateSpeed(originalSpeed); // Reset to original speed
            spriteRenderer.color = originalColor; // Ensure the color is reset to original
            if (debugMode) {
            Debug.Log("Stun ended. Restored original state.");

            }
        }

        isStunned = false; // Reset the stun flag
    }
}

