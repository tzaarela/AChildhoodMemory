using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
	private float frames;
	private Vector3 movement, currentPos;

	public float motion = 1f, steps = 1f;

	private void Start()
	{
		motion *= 0.003f;
		steps *= 0.02f;
	}


	private void FixedUpdate()
	{
		transform.position += new Vector3(0, Mathf.Sin(frames), 0) * motion;
		frames += steps;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
			Destroy(gameObject);
	}
}