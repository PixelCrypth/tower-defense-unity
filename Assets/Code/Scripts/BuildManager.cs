using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;
    private int SelectedTower = 0;

    private void Awake()
    {
        main = this;
    }

    public Tower GetSelectedTower()
    {
        return towers[SelectedTower];
    }

    public void SetSelectTower(int _selectedTower)
    {
        SelectedTower = _selectedTower;
    }

    public Tower GetTurretDataByName(string turretName)
    {
        foreach (Tower tower in towers)
        {
            if (tower.name == turretName)
            {
                return tower;
            }
        }
        return null; // or throw an exception if preferred
    }
}
