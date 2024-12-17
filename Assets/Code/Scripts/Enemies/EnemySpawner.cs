using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner main;

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs; // Corrected to match the array definition

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecondCap = 15f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive; // Variable to track enemies alive
    private int enemiesLeftToSpawn;
    private float eps; // enemies per second
    private bool isSpawning = false;

    private void Awake()
    {
        main = this;
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;
        // if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0){
        if (timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++; // Increment enemiesAlive
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--; // Decrement enemiesAlive
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
    }

    public int GetCurrentWave()
    {
        // Return the current wave number
        return currentWave;

    }

    private void EndWave()
    {
        LevelManager.main.IncreaseCurrencyGold(1); // Call the RewardGoldEndRound method

        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;

        StartCoroutine(StartWave());
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        // Pick an enemy from the enemyPrefabs array
        GameObject prefabToSpawn = enemyPrefabs[index];
        // Instantiate the selected enemy prefab
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, difficultyScalingFactor), 0f, enemiesPerSecondCap);
    }

    // StartNextWave coroutine
    // private IEnumerator StartNextWave()
    // {
    //     isSpawning = false; // Stop spawning temporarily
    //     yield return new WaitForSeconds(timeBetweenWaves); // Wait for the specified time between waves
    //     currentWave++; // Increase the wave count
    //     Debug.Log("Wave " + currentWave); // Log the current wave
    //     StartWave(); // Start the next wave
    // }

    public void ResetWaves()
    {
        StopAllCoroutines(); // Stop all coroutines to reset the wave properly
        currentWave = 1; // Reset the wave count to 1
        enemiesAlive = 0; // Reset the count of alive enemies
        enemiesLeftToSpawn = 0; // Reset the count of enemies left to spawn
        timeSinceLastSpawn = 0f; // Reset the spawn timer
        isSpawning = false; // Stop spawning
    }

    public void RestartGame()
    {
        ResetWaves(); // Reset the waves
        StartCoroutine(StartWave()); // Start the first wave
    }


}
