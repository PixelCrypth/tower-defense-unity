using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Singleton instance
    public static LevelManager main;
    
    // Path and start point for the level
    public Transform[] path;
    public Transform startPoint;

    [Header("Attributes")]
    [SerializeField] private int StartingSilver = 50;
    [SerializeField] private int StartingGold = 1;

    // Currency variables
    public int silver;
    public int gold;

    private void Awake() {
        // Set singleton instance
        main = this;
    }

    private void Start() {
        // Initialize silver and gold
        silver = StartingSilver;
        gold = StartingGold;
    }

    // Increase silver by a specified amount
    public void IncreaseCurrencySilver(int amount) {
        silver += amount;
    }

    public void ResetCurrencyToStartingValues() {
        silver = StartingSilver;
        gold = StartingGold;
    }

    public void ResetCurrencyToZero() {
        silver = 0;
        gold = 0;
    }



    // Increase gold by a specified amount
    public void IncreaseCurrencyGold(int amount) {
        gold += amount;
    }

    // Spend silver if enough is available
    public bool SpendCurrencySilver(int amount) {
        if (amount <= silver){
            silver -= amount;
            return true;
        } else {
            Debug.Log("Not enough silver");
            return false;
        }
    }

    // Spend gold if enough is available
    public bool SpendCurrencyGold(int amount) {
        if (amount <= gold){
            gold -= amount;
            return true;
        } else {
            Debug.Log("Not enough gold");
            return false;
        }
    }

    

}
