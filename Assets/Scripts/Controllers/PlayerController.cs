using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	PlayerParticles playerParticles;

	public float speed = 1;
	public float maxSpeed = 10;
	public float jumpStrength = 4;
	public float jumpSensitivity = 1f;
	public LayerMask groundLayer;

	private float horizontalInput;
	private bool isJumping;
	private bool isGrounded;
	private bool isDashing;
	private Rigidbody2D rb;


	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		//Move
		rb.velocity += new Vector2(horizontalInput * speed, 0);
		var clampedVector = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
		rb.velocity = clampedVector;

		//Check if grounded       
		var hit = Physics2D.Raycast(transform.position, Vector2.down, jumpSensitivity, groundLayer);

		isGrounded = false;
		if (hit.collider != null)
		{
			Debug.Log("IsGrounded...");
			isGrounded = true;
		}

		//Jump
		if(isJumping && isGrounded)
		{
			rb.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);

			if (playerParticles == null)
            {
				Debug.LogWarning("dustParticle on jump is not set!");
				return;
            }

			Instantiate(playerParticles.jumpDustParticleSystem, transform.position, Quaternion.identity);
			Debug.Log("IsJumping...");
		}
	}

	public void OnMove(InputAction.CallbackContext inputAction)
	{
		Debug.Log("Moving...");
		horizontalInput = inputAction.ReadValue<float>();
	}

	public void OnJump(InputAction.CallbackContext inputAction)
	{
		isJumping = inputAction.ReadValue<float>() == 1f;
	}

	public void OnDash(InputAction.CallbackContext inputAction)
	{
		isDashing = inputAction.ReadValue<float>() == 1f;
	}

	public void Die()
	{
		GameController.Instance.OnPlayerDie.Invoke(this);
	}
}
