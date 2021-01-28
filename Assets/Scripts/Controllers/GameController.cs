using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameController", menuName = "GameController")]
public class GameController : ScriptableObject
{
    public static GameController Instance;

    [Header("Platforms")]
    public float platformCount = 10;
    public float platformDistance = 10;

    public Action OnPlayerDie;
    public Action OnGameCompleted;

    private Spawnpoint spawnpoint;

    public void Init()
    {
        if (Instance != this)
            Instance = this;

        OnPlayerDie += HandleOnPlayerDie;
        OnGameCompleted += HandleOnGameCompleted;

        spawnpoint = FindObjectOfType<Spawnpoint>();

        if (spawnpoint == null)
            Debug.LogError("No spawnpoint found in scene!");
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
