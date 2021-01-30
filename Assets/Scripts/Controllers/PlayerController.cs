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

	[Header("Player")]
	public Vector3 playerPosition;
	public float speed = 1;
	public float maxSpeed = 10;
	public float jumpStrength = 4;
	public float jumpSensitivity = 1f;
	public float doubleJumpStrength = 4;
	public float doubleJumpDuration = 1;
	public float dashStrength = 3;
	public float wallJumpStrength = 40;
	public float wallJumpVerticalBoost = 10;
	public float wallSensitivity = 1f;
	public float fallMultiplier = 2.5f;
	public LayerMask groundLayer, wallLayer;

	[Header("Animations")]
	public float walkSensitivity = 1f;

	[Header("Particles")]
	[SerializeField] PlayerParticles playerParticles;
	
	[Header("UI")]
	public GameObject checkpointText = null;
	
	private float horizontalInput;
	private bool isCornered;
	private bool isJumping;
	private bool isGrounded;
	private bool isDashing;
	private bool canWallJump;
	private bool canDoubleJump;
	private bool wallJumpLeft, wallJumpRight;
	private Rigidbody2D rb;
	private Animator animator;
	private SpriteRenderer spriteRenderer;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	private void FixedUpdate()
	{
		playerPosition = transform.position;
		Move();
		CheckIfGrounded();
		//CheckIfCornered();
		CheckForWalls();
		ImproveJump();
		
		if (isGrounded && Mathf.Abs(rb.velocity.x) > walkSensitivity)
			animator.SetBool("isMoving", true);
		else
			animator.SetBool("isMoving", false);

		if (rb.velocity.x < 0)
			spriteRenderer.flipX = true;
		else
			spriteRenderer.flipX = false;

	}

	private void Move() 
	{
		if (Mathf.Abs(rb.velocity.x) < maxSpeed)
			rb.velocity += new Vector2(horizontalInput * speed, 0);
	}

	private void CheckIfGrounded()
	{
		var hitGround = Physics2D.Raycast(transform.position, Vector2.down, jumpSensitivity, groundLayer);

		isGrounded = false;
		if (hitGround.collider != null)
		{
			isGrounded = true;
		}
	}
	
	private void CheckForWalls()
	{
		var hitWallLeft = Physics2D.Raycast(transform.position, Vector2.left, wallSensitivity, wallLayer);
		var hitWallRight = Physics2D.Raycast(transform.position, Vector2.right, wallSensitivity, wallLayer);

		canWallJump = false;
		if (hitWallLeft.collider != null || hitWallRight.collider != null)
		{
			canWallJump = true;
			if (rb.velocity.y < -0.05 && !isGrounded)
			{
				sideWallLeft.enabled = true;
				sideWallRight.enabled = true;
			}
			else
			{
				sideWallLeft.enabled = false;
				sideWallRight.enabled = false;
			}
		}

		wallJumpLeft = hitWallLeft.collider != null;
		wallJumpRight = hitWallRight.collider != null;

	}

	private void CheckIfCornered()
	{
		isCornered = canWallJump && isGrounded;

		if(sideWallLeft == null || sideWallRight == null)
		{
			Debug.LogError("SideWalls not assigned in PlayerController");
			return;
		}
		
		sideWallLeft.enabled = !isCornered;
		sideWallRight.enabled = !isCornered;
	}
	
	private void Jump(InputAction.CallbackContext jumpAction)
	{
		//if (!isJumping)
		//	return;
		

		if (canWallJump && !isGrounded && jumpAction.performed)
		{
			if (wallJumpLeft)
			{
				AudioController.Instance.PlaySound("WallJump");

				animator.SetTrigger("onSideJump");
				rb.velocity = new Vector2(4, 4);
				rb.AddForce(new Vector2(wallJumpStrength, wallJumpVerticalBoost), ForceMode2D.Impulse);
			}
			else if (wallJumpRight)
			{
				AudioController.Instance.PlaySound("WallJump");
				animator.SetTrigger("onSideJump");
				rb.velocity = new Vector2(-4, 4);
				rb.AddForce(new Vector2(wallJumpStrength * -1, wallJumpVerticalBoost), ForceMode2D.Impulse);
			}

			Instantiate(playerParticles.jumpDustParticleSystem, transform.position, Quaternion.identity);
		}

		else if (isGrounded && jumpAction.performed)
		{
			//Debug.Log("IsJumping...");
			AudioController.Instance.PlaySound("Jump");
			rb.velocity += Vector2.up * jumpStrength;
			animator.SetTrigger("onJump");
			//rb.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);

			if (playerParticles.jumpDustParticleSystem == null)
			{
				Debug.LogWarning("dustParticle on jump is not set!");
				return;
			}

			Instantiate(playerParticles.jumpDustParticleSystem, transform.position, Quaternion.identity);
			Debug.Log("IsJumping...");
		}

		else if (canDoubleJump && jumpAction.performed)
		{
			AudioController.Instance.PlaySound("Jump");
			rb.velocity = new Vector2(rb.velocity.x, 0);
			rb.velocity += Vector2.up * doubleJumpStrength;
			canDoubleJump = false;
			animator.SetTrigger("onJump");
		}
	}

	private void ImproveJump()
	{
		if (rb.velocity.y < 0)
			rb.velocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
	}
	
public void GiveDoubleJump()
	{
		canDoubleJump = true;
		StartCoroutine(PowerDrainCoroutine());
	}

	public IEnumerator PowerDrainCoroutine()
	{
		yield return new WaitForSeconds(doubleJumpDuration);
		canDoubleJump = false;
	}


	public void OnMove(InputAction.CallbackContext inputAction)
	{
		//Debug.Log("Moving...");
		horizontalInput = inputAction.ReadValue<float>();
	}

	public void OnJump(InputAction.CallbackContext jumpAction)
	{
		Jump(jumpAction);
	}

	public void OnDash(InputAction.CallbackContext inputAction)
	{
		isDashing = inputAction.ReadValue<float>() == 1f;
		if (isDashing && isGrounded)
		{
			if (horizontalInput > 0.01)
			{
				animator.SetTrigger("onDash");
				AudioController.Instance.PlaySound("Dash");
				rb.AddForce(new Vector2(dashStrength, 0), ForceMode2D.Impulse);
			}
			else if (horizontalInput < -0.01)
			{
				animator.SetTrigger("onDash");
				AudioController.Instance.PlaySound("Dash");
				rb.AddForce(new Vector2(dashStrength * -1, 0), ForceMode2D.Impulse);
			}
		}
	}

	public void Die()
	{
		GameController.Instance.OnPlayerDie.Invoke(this);
	}

	public void StartCheckpointReached()
	{
		checkpointText.SetActive(true);
		StartCoroutine(CheckpointReached());
	}
	
	public IEnumerator CheckpointReached()
	{
		float time = 0;
		float duration = 0.1f;
		float startValue = 0.6f;
		float endValue = 0.1f;
		Time.timeScale = startValue;

		while (time < duration)
		{
			Time.timeScale = (Mathf.Lerp(startValue, endValue, time / duration));
			time += Time.deltaTime;
			yield return null;
		}

		yield return new WaitForSeconds(0.1f);
		Time.timeScale = 1;
		checkpointText.SetActive(false);
	}
}