using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electro_Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb; // Reference to the Rigidbody2D component
    [SerializeField] private Animator animator; // Reference to the Animator component

    [Header("Attributes")]
    private Transform target; // Target to follow
    private float bulletSpeed;
    private float bulletDamage;
    private float stunDuration;
    private Color stunColor;
    private float bulletLifetime; // Lifetime of the bullet
    private float bulletRange; // Range of the bullet


    // Set the target for the bullet
    public void SetTarget(Transform _target)
    {
        target = _target;
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
        Debug.Log("Bullet range set to: " + bulletRange);
        Invoke("DestroyBullet", bulletLifetime); // Destroy the bullet after the calculated lifetime
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

    // Set the stun duration for the bullet
    public void SetStunDuration(float _stunDuration)
    {
        stunDuration = _stunDuration;
    }

    // Set the stun color for the bullet
    public void SetStunColor(Color _stunColor)
    {
        stunColor = _stunColor;
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
        Health health = other.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(bulletDamage); // Deal damage

            // Check if the enemy has a "Stunnable" component and stun it (This is a script within enemy)
            Stunnable enemy = other.gameObject.GetComponent<Stunnable>();
            if (enemy != null)
            {
                enemy.Stun(stunDuration, stunColor); // Start the stun process
            }
        }
        Destroy(gameObject); // Destroy the bullet
    }
}
