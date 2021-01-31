using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    private void Awake()
    {
        Debug.Log("TestAwake");
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        
       ///*DontDestroyOnLoad*/(gameObject);
    }

    public void ChangeScene(string name)
    {
        Debug.Log("Trying to change scene...");
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Debug.Log("Trying to quit...");
        Application.Quit();
    }
}
