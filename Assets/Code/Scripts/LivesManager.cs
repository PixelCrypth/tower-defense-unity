using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LivesManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private int lives = 3;
    [SerializeField] private GameObject[] lifeImages; // Each life has a LiveParticle script attached
    [SerializeField] private Animator[] livesAnimators;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI currentWaveTextObject;
    private bool[] lifeActive;


    public static LivesManager main;

    void Start()
    {
        main = this;
        gameOverUI.SetActive(false);
        lifeActive = new bool[lifeImages.Length];
        for (int i = 0; i < lifeActive.Length; i++)
        {
            lifeActive[i] = true; // All lives are active at the start
        }
    }

    public void DecreaseLives(int amount)
    {
        StartCoroutine(HandleLifeLoss(amount)); // Process multiple life losses in one coroutine
    }

    public void ResetLives()
    {
        Debug.Log("Resetting lives");
        lives = lifeImages.Length;

        for (int i = 0; i < lifeImages.Length; i++)
        {
            GameObject lifeImage = lifeImages[i];
            lifeImage.SetActive(true); // Activate life image first
            LiveParticle liveParticle = lifeImage.GetComponent<LiveParticle>();
            lifeActive[i] = true; // Reset lifeActive

            if (liveParticle != null)
            {
                liveParticle.StopParticles(); // Stop particles
            }

            Animator animator = lifeImage.GetComponent<Animator>();
            if (animator != null)
            {
                animator.ResetTrigger("LifeLoss");
                animator.ResetTrigger("LifeGain");
                animator.Play("Idle"); // Return to idle animation
                animator.SetTrigger("LifeGain");
            }

            StartCoroutine(FadeInAndActivate(lifeImage));
        }
    }



    private IEnumerator HandleLifeLoss(int amount)
    {
        int livesLost = 0;

        for (int i = 0; i < lifeImages.Length && livesLost < amount; i++)
        {
            if (lifeActive[i]) // Use lifeActive instead of activeSelf
            {
                lifeActive[i] = false; // Lock this life so no other coroutine can use it
                Animator animator = lifeImages[i].GetComponent<Animator>();
                LiveParticle liveParticle = lifeImages[i].GetComponent<LiveParticle>();

                if (animator != null)
                {
                    animator.SetTrigger("LifeLoss");
                    Debug.Log("Life lost on crystal " + i);

                    if (liveParticle != null)
                    {
                        liveParticle.PlayParticles();
                    }

                    StartCoroutine(FadeOutAndDeactivate(lifeImages[i]));
                    lives--;
                    livesLost++;

                    if (lives <= 0)
                    {
                        HandleGameOver();
                        yield break;
                    }
                }
            }
        }

        yield return null; // Ensure coroutine finishes properly
    }

    private void HandleGameOver()
    {
        Debug.Log("Game Over");
        gameOverUI.SetActive(true); // Show GameOver UI

        if (currentWaveTextObject != null)
        {
            currentWaveTextObject.text = "You reached wave " + EnemySpawner.main.GetCurrentWave();
        }

        DestroyAllEnemiesAndTurrets();

        LevelManager.main.ResetCurrencyToZero();
        EnemySpawner.main.ResetWaves();
    }

    private void DestroyAllEnemiesAndTurrets()
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int turretLayer = LayerMask.NameToLayer("Turret");

        GameObject[] objects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objects)
        {
            if (obj.layer == enemyLayer || obj.layer == turretLayer)
            {
                Destroy(obj);
            }
        }
    }

    private IEnumerator FadeOutAndDeactivate(GameObject obj)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = obj.AddComponent<CanvasGroup>();
        }

        for (float t = 1f; t > 0f; t -= Time.deltaTime)
        {
            canvasGroup.alpha = t;
            yield return null;
        }

        obj.SetActive(false);
    }

    private IEnumerator FadeInAndActivate(GameObject obj)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = obj.AddComponent<CanvasGroup>();
        }

        obj.SetActive(true); // Make sure the life is visible
        for (float t = 0f; t <= 1f; t += Time.deltaTime)
        {
            canvasGroup.alpha = t;
            yield return null;
        }
    }
}
