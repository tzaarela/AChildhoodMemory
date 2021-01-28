using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameController", menuName = "GameController")]
public class GameController : ScriptableObject
{
    [Header("Platforms")]
    public float platformCount = 10;
    public float playformDistance = 10;

    public Action OnPlayerDie;
    public Action OnGameCompleted;
     
    public static GameController Instance;

    public void Init()
    {
        if (Instance != this)
            Instance = this;

        OnPlayerDie += HandleOnPlayerDie;
        OnGameCompleted += HandleOnGameCompleted;
    }

    private void HandleOnGameCompleted()
    {
        Debug.Log("Game completed!");
    }

    private void HandleOnPlayerDie()
    {
        Debug.Log("Player died!");
    }
}
