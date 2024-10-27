using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    enum PlayerState { Idle, Running, Airborne, Dashing, Attacking}

    PlayerState state;
    bool stateComplete;

    public Animator animator;
    public Rigidbody2D rb;
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;
    

    public float moveSpeed;
    public float jumpHeight;
    public float acceleration;
    
    public bool grounded;
    
    [Range(0f, 1f)]
    public float groundDecay;

    float xInput;
    


    // Update is called once per frame
    void Update()
    {
        CheckInput();
        HandleJump();
        if(stateComplete)
        {
            SelectState();
        }
        UpdateState();
    }

    private void FixedUpdate()
    {
        CheckGround();
        ApplyFriction();
        HandleMovement();
    }

    private void UpdateState()
    {
        switch (state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Running:
                UpdateRunning();
                break;
            case PlayerState.Airborne:
                UpdateAirborne();
                break;
            case PlayerState.Dashing:

                break;
            case PlayerState.Attacking:

                break;

        }

    }

    private void SelectState()
    {
        stateComplete = false;

        if(grounded)
        {
            if (xInput == 0)
            {
                state = PlayerState.Idle;
                StartIdle();
            }
            else
            {
                state = PlayerState.Running;
                StartRunning();
            }
        }
        else { 
            state = PlayerState.Airborne;
            StartAirborne();
        }
    }

    private void UpdateIdle()
    {
        if (xInput != 0)
        {
            stateComplete = true;
        }
    }

    private void UpdateRunning()
    {
        if (!grounded || xInput == 0)
        {
            stateComplete = true;
        }
    }

    private void UpdateAirborne()
    {
        if (grounded)
        {
            stateComplete = true;
        }
    }

    private void StartIdle()
    {
        animator.Play("idle");
    }

    private void StartRunning()
    {
        animator.Play("running");
    }


    private void StartAirborne()
    {
        animator.Play("jump");
    }

    private void CheckInput()
    {
        xInput = Input.GetAxis("Horizontal");
        
    }

    private void HandleMovement()
    {
        if(Mathf.Abs(xInput) > 0)
        {

            // Increments velocity by acceleration
            float increment = xInput * acceleration;
            float newSpeed = Mathf.Clamp(rb.velocity.x + increment, -moveSpeed, moveSpeed);

            rb.velocity = new Vector2(newSpeed, rb.velocity.y);

            DirectionInput();
        }
    }

    private void DirectionInput()
    {
        float direction = Mathf.Sign(xInput);
        transform.localScale = new Vector3(direction, 1, 1);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
    }


    private void CheckGround()
    {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    private void ApplyFriction()
    {
        if (grounded && xInput == 0 && rb.velocity.y <= 0) 
        {
            rb.velocity *= groundDecay;
        }
    }

}
