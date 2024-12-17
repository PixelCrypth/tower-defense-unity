using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret_effects : MonoBehaviour
{

    private static turret_effects _instance;

    public static turret_effects Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<turret_effects>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("turret_effects");
                    _instance = go.AddComponent<turret_effects>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    public void StartStunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}

