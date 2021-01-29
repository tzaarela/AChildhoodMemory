using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance;

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
		CVCam.OnTargetObjectWarped(playerController.transform, Vector3.down * 100);
	}
}
