using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public Action OnPlayerDie;
    public Action OnGameCompleted;
     
    public static GameController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        OnPlayerDie += HandleOnPlayerDie;
        OnGameCompleted += HandleOnGameCompleted;
    }

    private void HandleOnGameCompleted()
    {
        throw new NotImplementedException();
    }

    private void HandleOnPlayerDie()
    {
        throw new NotImplementedException();
    }
}
