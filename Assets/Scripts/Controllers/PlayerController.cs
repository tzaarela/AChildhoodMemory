using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    public float maxSpeed = 10;
    public float jumpStrength = 4;
    public float jumpSensitivity = 1f;
    public float dashStrength = 3;
    public float wallJumpStrength = 40;
    public LayerMask groundLayer;

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
        Debug.Log(rb.velocity);
        // var clampedVector = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        // Debug.Log(clampedVector);
        // rb.velocity = clampedVector;
        
        
        
        //Check if grounded       
        var hit = Physics2D.Raycast(transform.position, Vector2.down, jumpSensitivity, groundLayer);

        isGrounded = false;
        if (hit.collider != null)
        {
            //Debug.Log("IsGrounded...");
            isGrounded = true;
        }

        //Jump
        if(isJumping && isGrounded)
        {
            //Debug.Log("IsJumping...");
            AudioController.Instance.PlaySound("Jump");
            rb.velocity += Vector2.up * jumpStrength;
            //rb.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
        }
        
        //Better jump (increased velocity when moving down)
        if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
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
                rb.AddForce(new Vector2(wallJumpStrength, 15), ForceMode2D.Impulse);
            }
            else if (transform.position.x > 0)
            {
                AudioController.Instance.PlaySound("WallJump");
                rb.velocity = new Vector2(-4, 4);
                rb.AddForce(new Vector2(wallJumpStrength * -1, 15), ForceMode2D.Impulse);
            }
        }
    }
    
    public void OnDash(InputAction.CallbackContext inputAction)
    {
        isDashing = inputAction.ReadValue<float>() == 1f;
        if(isDashing && isGrounded)
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

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!isGrounded && other.transform.CompareTag("Wall"))
            canWallJump = true;
        else
            canWallJump = false;
    }
    
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.CompareTag("Wall"))
            canWallJump = false;
    }

    public void Die()
    {
        GameController.Instance.OnPlayerDie.Invoke(this);
    }
}