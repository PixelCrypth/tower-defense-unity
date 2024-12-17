using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")] // Correct attribute name
    [SerializeField] private Rigidbody2D rb; // Correct attribute name

    [SerializeField] private float speed = 2f; // Correct attribute name

    public float GetSpeed()
    {
        return speed;
    }

    private Transform target;
    private int pathIndex = 0;

    // Start is called before the first frame update
    private void Start()
    {
        target = LevelManager.main.path[pathIndex];
    }

    // Update is called once per frame
    private void Update()
    { // Added void return type
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == LevelManager.main.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke(); // call onEnemyDestroy event on enemyspawner
                Destroy(gameObject);
                Debug.Log("Enemy reached the end of the path, player lose health");
                LivesManager.main.DecreaseLives(1);
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
    }

    // FixedUpdate is called once per physics frame
    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized; // Correct capitalization

        rb.velocity = direction * speed; // Changed moveSpeed to speed
    }

    public void UpdateSpeed(float newSpeed)
    {
        speed = newSpeed;
    }


}
