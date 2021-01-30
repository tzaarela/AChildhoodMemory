using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointArea : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			Debug.Log("CheckPoint Action!");
			GameController.Instance.OnCheckpointReached(other.GetComponent<PlayerController>());
			Destroy(gameObject);
		}
	}
}
