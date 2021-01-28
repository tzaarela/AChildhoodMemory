using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void PlaySound()
    {

    }

    public void PlayBackgroundMusic()
    {

    }
}
