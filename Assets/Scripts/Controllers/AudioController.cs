using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	public static AudioController Instance;
	
	[Header("Effects")]
	[SerializeField] AudioClip run = null;
	[SerializeField] float runVolume = 1.0f;
	[SerializeField] AudioClip jump = null;
	[SerializeField] float jumpVolume = 1.0f;
	[SerializeField] AudioClip dash = null;
	[SerializeField] float dashVolume = 1.0f;
	[SerializeField] AudioClip wallJump = null;
	[SerializeField] float wallJumpVolume = 1.0f;

	[Header("Music")]
	[SerializeField]
	public List<float> volumes = new List<float>
	{
		0.8f,
		0.8f,
		0.8f
	};

	public List<AudioClip> musicClips;

	public AudioSource effectsAudioSource = null;
	public AudioSource musicAudioSource;
	//Not necessary? Add Music directly to AudioSource...  Answer: -I think it�s better right? To avoid clipping etc. We should try the inbuilt mixer tough //tzaa

	private int currentSongIndex = 0;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);
		
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		if (effectsAudioSource == null)
			effectsAudioSource = GetComponent<AudioSource>();
	}

    private void Update()
    {
        if (!musicAudioSource.isPlaying)
        {
			PlayNextSong();
        }
    } 

public void PlaySound(string soundClip)
	{
		switch (soundClip)
		{
			case "Run":
				if (run == null)
					return;
				effectsAudioSource.PlayOneShot(run, runVolume);
				break;
			case "Jump":
				if (jump == null)
					return;
				effectsAudioSource.PlayOneShot(jump, jumpVolume);
				break;
			case "Dash":
				if (dash == null)
					return;
				effectsAudioSource.PlayOneShot(dash, dashVolume);
				break;
			case "WallJump":
				if (wallJump == null)
					return;
				effectsAudioSource.PlayOneShot(wallJump, wallJumpVolume);
				break;
		}
	}

	//public void PlayBackgroundMusic(int index) // Not necessary?
	//{
	//	musicAudioSource.
	//}

	public void PlayNextSong()
    {
		musicAudioSource.Stop();
		currentSongIndex++;
		if (currentSongIndex >= musicClips.Count)
			currentSongIndex = 0;

		musicAudioSource.PlayOneShot(musicClips[currentSongIndex], volumes[currentSongIndex]);
    }
}
