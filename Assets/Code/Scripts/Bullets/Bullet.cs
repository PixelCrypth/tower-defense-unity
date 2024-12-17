using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb; // Reference to the Rigidbody2D component

    [Header("Attributes")]
    private Transform target; // Target to follow
    private float bulletSpeed; // Speed of the bullet
    private float bulletDamage; // Damage of the bullet
    private float bulletRange; // Range of the bullet
    private float bulletLifetime; // Lifetime of the bullet

    [Header("Debug")]
    [SerializeField] private bool debugMode = false; // Enable or disable debug logs

    // Set the target for the bullet
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    // Set the damage for the bullet
    public void SetDamage(float _damage)
    {
        bulletDamage = _damage;
    }

    // Set the speed for the bullet
    public void SetSpeed(float _speed)
    {
        bulletSpeed = _speed;
    }

    // Set the range for the bullet
    public void SetRange(float _range)
    {
        bulletRange = _range;

        // Formula to calculate the bullet's lifetime based on its range and speed
        // Adding a minimum lifetime for cleanup purposes
        // bulletLifetime = (bulletRange / bulletSpeed) + additionalLifetime
        // example: bulletRange = 10, bulletSpeed = 5 -> bulletLifetime = (10 / 5) + 3 = 5
        // example2: bulletRange = 20, bulletSpeed = 5 -> bulletLifetime = (20 / 5) + 3 = 7
        float additionalLifetime = 3.0f; // Additional lifetime for cleanup
        bulletLifetime = (bulletRange / bulletSpeed) + additionalLifetime; // Calculate the lifetime based on range, speed, and additional time
        if (debugMode) Debug.Log("Bullet range set to: " + bulletRange);
        Invoke("DestroyBullet", bulletLifetime); // Destroy the bullet after the calculated lifetime
    }

    // Update the bullet's velocity towards the target
    private void FixedUpdate()
    {
        if (!target) return;
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * bulletSpeed;
    }

    // Handle collision with other objects
    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<Health>().TakeDamage(bulletDamage); // Deal damage
        Destroy(gameObject); // Destroy the bullet
        if (debugMode) Debug.Log("Bullet hit: " + other.gameObject.name); // Log the hit
    }

    // Destroy the bullet
    private void DestroyBullet()
    {
        Destroy(gameObject);
        if (debugMode) Debug.Log("Bullet destroyed");
    }
}
