using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	[SerializeField]
	public int spawnSegment;
	[SerializeField]
	public float spawnPointStep = 5f;
	[SerializeField] 
	private GameObject[] platforms;
	[SerializeField]
	private float spawnPosY;
	[SerializeField]
	private Vector3 spawnPoint;


	private void Start()
	{
		spawnSegment = 1;
		spawnPosY = 5f;
		spawnPoint = new Vector3();

		platforms = new GameObject[1];

		for (int i = 0; i < platforms.Length; i++)
		{
			spawnPosY += spawnPointStep;
			spawnSegment ++;
			spawnPoint.y += spawnPosY;

			Instantiate(platforms[i], spawnPoint, Quaternion.identity);
		}
	}

	private void Update()
	{
		
	}
}
