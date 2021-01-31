using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	public Action<PlayerController> OnCheckpointReached;
	public Action<PlayerController> OnGameCompleted;

	public GameObject confetti;

	[SerializeField]
	private int checkpointsReached;

	private Camera mainCamera;
	private Spawnpoint[] spawnpoints;
	private Vector3 cameraStartPosition;
	private Coroutine WaitForCamera;

	public void Init()
	{
		if (Instance != this)
			Instance = this;

		checkpointsReached = 1;
		OnPlayerDie += HandleOnPlayerDie;
		OnCheckpointReached += HandleOnCheckpointReached;
		OnGameCompleted += HandleOnGameCompleted;

		spawnpoints = FindObjectsOfType<Spawnpoint>().OrderBy(x => x.id).ToArray();
		if (spawnpoints.Length == 0)
        {
			Debug.LogError("No spawnpoints found in scene!");
        }

		mainCamera = Camera.main;
		cameraStartPosition = mainCamera.transform.position;
	}

	private void HandleOnCheckpointReached(PlayerController playerController)
	{
		Debug.Log("Checkpoint reached!");
		checkpointsReached += 1;
		Instantiate(confetti, playerController.transform.position, Quaternion.identity);
		playerController.StartCheckpointReached();
	}
	
	private void HandleOnGameCompleted(PlayerController playerController)
	{
		Debug.Log("Game completed!");
		//playerController.transform.position = spawnpoints[0].transform.position;
		//CameraController.Instance.ResetCameraPosition(playerController);
	}

	private void HandleOnPlayerDie(PlayerController playerController)
	{
		Debug.Log("Player died!");
		playerController.transform.position = spawnpoints[checkpointsReached].transform.position;
		CameraController.Instance.ResetCameraPosition(playerController);
	}
}