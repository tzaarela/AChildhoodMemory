using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{ 
    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private GameController levelController;

    public void Awake()
    {
        if (GameController.Instance == null)
        {
            gameController.Init();
            DontDestroyOnLoad(gameObject);
        }
    }
}
