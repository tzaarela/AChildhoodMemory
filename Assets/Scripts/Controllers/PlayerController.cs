using System;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private SurfaceEffector2D sideWallLeft, sideWallRight;

	public float speed = 1;
	public float maxSpeed = 10;
	public float jumpStrength = 4;
	public float jumpSensitivity = 1f;
	public float dashStrength = 3;
	public float wallJumpStrength = 40;
	public float wallJumpVerticalBoost = 10;
	public float wallSensitivity = 1f;
	public LayerMask groundLayer, wallLayer;

	[SerializeField]
	PlayerParticles playerParticles;

	public float fallMultiplier = 2.5f;

	private float horizontalInput;
	private bool isJumping;
	private bool isGrounded;
	private bool isDashing;
	private bool canWallJump;
	private Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		//Move
		if (Mathf.Abs(rb.velocity.x) < maxSpeed)
			rb.velocity += new Vector2(horizontalInput * speed, 0);
		// var clampedVector = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
		// rb.velocity = clampedVector;
		
		//Check if grounded       
		var hitGround = Physics2D.Raycast(transform.position, Vector2.down, jumpSensitivity, groundLayer);

		isGrounded = false;
		if (hitGround.collider != null)
		{
			//Debug.Log("IsGrounded...");
			isGrounded = true;
		}

		//Wall Jump
		var hitWallLeft = Physics2D.Raycast(transform.position, Vector2.left, wallSensitivity, wallLayer);
		var hitWallRight = Physics2D.Raycast(transform.position, Vector2.right, wallSensitivity, wallLayer);

		canWallJump = false;
		if (hitWallLeft.collider != null || hitWallRight.collider != null)
		{
			canWallJump = true;
		}

		//Jump
		if(isJumping && isGrounded)
		{
			//Debug.Log("IsJumping...");
			AudioController.Instance.PlaySound("Jump");
			rb.velocity += Vector2.up * jumpStrength;
			//rb.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);

            if (playerParticles.jumpDustParticleSystem == null)
            {
                Debug.LogWarning("dustParticle on jump is not set!");
                return;
            }

			Instantiate(playerParticles.jumpDustParticleSystem, transform.position, Quaternion.identity);
			Debug.Log("IsJumping...");
		}
		
		//Better jump (increased velocity when moving down)
		if (rb.velocity.y < 0)
			rb.velocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);

		//When in Corner
		bool isInCorner = canWallJump && isGrounded;

		if(sideWallLeft == null || sideWallRight == null)
        {
			Debug.LogError("SideWalls not assigned in PlayerController");
			return;
        }

		sideWallLeft.enabled = !isInCorner;
		sideWallRight.enabled = !isInCorner;
	}

	public void OnMove(InputAction.CallbackContext inputAction)
	{
		//Debug.Log("Moving...");
		horizontalInput = inputAction.ReadValue<float>();
	}

	public void OnJump(InputAction.CallbackContext inputAction)
	{
		isJumping = inputAction.ReadValue<float>() == 1f;
		
		if (canWallJump && isJumping)
		{
			if (transform.position.x < 0)
			{
				AudioController.Instance.PlaySound("WallJump");
				rb.velocity = new Vector2(4, 4);
				rb.AddForce(new Vector2(wallJumpStrength, wallJumpVerticalBoost), ForceMode2D.Impulse);
			}
			else if (transform.position.x > 0)
			{
				AudioController.Instance.PlaySound("WallJump");
				rb.velocity = new Vector2(-4, 4);
				rb.AddForce(new Vector2(wallJumpStrength * -1, wallJumpVerticalBoost), ForceMode2D.Impulse);
			}
		}
	}
	
	public void OnDash(InputAction.CallbackContext inputAction)
	{
		isDashing = inputAction.ReadValue<float>() == 1f;
		if (isDashing && isGrounded)
		{
			if (horizontalInput > 0.01)
			{
				AudioController.Instance.PlaySound("Dash");
				rb.AddForce(new Vector2(dashStrength, 0), ForceMode2D.Impulse);
			}
			else if (horizontalInput < -0.01)
			{
				AudioController.Instance.PlaySound("Dash");
				rb.AddForce(new Vector2(dashStrength * -1, 0), ForceMode2D.Impulse);
			}
		}
	}

	public void Die()
	{
		GameController.Instance.OnPlayerDie.Invoke(this);
	}
}