using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Animator fadeOutAnim;

    public static UIController Instance;

    private void Awake()
    {
        if (Instance != this)
            Instance = this;
    }

    public void FadeOutWhite()
    {
        fadeOutAnim.SetTrigger("playAnimation");
    }

}
