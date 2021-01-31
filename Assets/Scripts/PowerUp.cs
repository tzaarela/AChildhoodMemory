using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
	private float frames;
	private CircleCollider2D circleCollider2D;

	public PowerType powerType;
	public GameObject powerOrb;
	public ParticleSystem powerPickupEffect;
	public float motion = 1f, steps = 1f;

	private void Start()
	{
		circleCollider2D = GetComponent<CircleCollider2D>();
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
        {
			var player = other.GetComponent<PlayerController>();

            switch (powerType)
            {
                case PowerType.AirDoubleJump:
					player.GiveDoubleJump();
                    break;
                case PowerType.LowGravity:
                    break;
                case PowerType.SlowMotion:
                    break;
                default:
                    break;
            }


			ToggleBody(false);
			powerPickupEffect.Play();
			StartCoroutine(WaitForRespawn());
        }
    }

	private IEnumerator WaitForRespawn()
    {
		yield return new WaitForSeconds(3);
		ToggleBody(true);
	}

	private void ToggleBody(bool toggle)
    {
		powerOrb.SetActive(toggle);
		circleCollider2D.enabled = toggle;
    }
}