using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Animator fadeOutAnim;

    public static UIController Instance;

    private bool isStarted;

    private void Awake()
    {
        if (Instance != this)
            Instance = this;
    }

    private void Update()
    {
        if(isStarted && fadeOutAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            SceneController.Instance.ChangeScene("Outro");
        }
    }

    public void FadeOutWhite()
    {
        fadeOutAnim.SetTrigger("playAnimation");
        isStarted = true;
        
    }

}
