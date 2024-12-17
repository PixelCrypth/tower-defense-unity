using System;
using UnityEngine;

/// Represents a tower with a name, cost, and associated prefab.
[Serializable]
public class Tower
{
    /// The name of the tower.
    public string name;

    /// The cost of the tower.
    public int silver;

    /// The cost of the tower.
    public int gold;

    /// The prefab associated with the tower.
    public GameObject prefab;

    /// Initializes a new instance of the Tower class.
    public Tower(string _name, int _silver, int _gold, GameObject _prefab)
    {
        name = _name;
        silver = _silver;
        gold = _gold;
        prefab = _prefab;
    }
}
