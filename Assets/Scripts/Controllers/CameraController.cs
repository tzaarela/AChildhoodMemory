using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance;
	public float cameraRespawnOffset = 1f;

	private Cinemachine.CinemachineVirtualCamera CVCam;

	private void Awake() 
	{
		if (Instance != this)
			Instance = this;
	}

	private void Start() 
	{
		CVCam = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();
	}

	public void ResetCameraPosition(PlayerController playerController) 
	{
		GameObject go = new GameObject();
		Transform target = go.transform;

		target.position = playerController.transform.position + new Vector3(0, cameraRespawnOffset, 0);
		CVCam.OnTargetObjectWarped(target, Vector3.down * 100);
	}
}
