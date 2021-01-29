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

	public Action<PlayerController> OnPlayerDie;
	public Action OnGameCompleted;

	public Spawnpoint spawnpoint;

	private Camera mainCamera;
	private Vector3 cameraStartPosition;
	private Coroutine WaitForCamera;

	public void Init()
	{
		if (Instance != this)
			Instance = this;

		OnPlayerDie += HandleOnPlayerDie;
		OnGameCompleted += HandleOnGameCompleted;

		spawnpoint = FindObjectOfType<Spawnpoint>();

		if (spawnpoint == null)
			Debug.LogError("No spawnpoint found in scene!");

		mainCamera = Camera.main;
		cameraStartPosition = mainCamera.transform.position;
	}

	private void HandleOnGameCompleted()
	{
		Debug.Log("Game completed!");
	}

	private void HandleOnPlayerDie(PlayerController playerController)
	{
		Debug.Log("Player died!");
		playerController.transform.position = GameController.Instance.spawnpoint.transform.position;
		CameraController.Instance.ResetCameraPosition(playerController);
	}
}
