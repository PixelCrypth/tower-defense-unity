using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject gameOverUI; // Reference to the GameOver UI canvas
    [SerializeField] private Button retryButton; // Reference to the Retry button

    private void Awake()
    {
        retryButton.onClick.AddListener(OnRetryButtonClicked);
    }

    private void OnRetryButtonClicked()
    {
        // Logic to restart the game
        Debug.Log("Retry button clicked");
        // Add your game restart logic here
        LivesManager.main.ResetLives();
        LevelManager.main.ResetCurrencyToStartingValues();
        
        gameOverUI.SetActive(false);
        EnemySpawner.main.RestartGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
