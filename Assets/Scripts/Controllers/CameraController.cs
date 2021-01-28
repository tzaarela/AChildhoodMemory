using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance;

	private Vector3 cameraStartPosition;

	private Cinemachine.CinemachineVirtualCamera cameraBrain;
	private BoxCollider2D killZoneCollider;

	private void Awake() 
	{
		if (Instance != this)
			Instance = this;
	}

	private void Start() 
	{
		cameraStartPosition = transform.position;
		cameraBrain = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();
		killZoneCollider = GetComponentInChildren<BoxCollider2D>();
	}

	public void ResetCameraPosition(PlayerController playerController) 
	{
		// killZoneCollider.gameObject.SetActive(false);
		// transform.position = cameraStartPosition;
		cameraBrain.enabled = false;
		cameraBrain.transform.SetPositionAndRotation(cameraStartPosition, Quaternion.identity);
		playerController.transform.position = GameController.Instance.spawnpoint.transform.position;
		cameraBrain.enabled = true;
		// StartCoroutine(CoWaitForCamera(playerController));
	}

	private IEnumerator CoWaitForCamera(PlayerController playerController) 
	{
		yield return new WaitForSeconds(3);
		// killZoneCollider.gameObject.SetActive(true);
	}
}
