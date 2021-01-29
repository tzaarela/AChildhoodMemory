using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinArea : MonoBehaviour
{
	// private PlayerController playerController;

	// private void Start()
	// {
	// 	playerController = GetComponent
	// }
	
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			Debug.Log("You Win!");
			GameController.Instance.OnGameCompleted(col.GetComponent<PlayerController>());
		}
	}
}
