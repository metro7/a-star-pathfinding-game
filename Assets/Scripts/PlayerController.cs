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
    public GameObject fallDetector;
    
    public float moveSpeed;
    public float jumpHeight;
    public bool grounded;
    private Vector3 respawnPoint;
    float xInput;
    [Range(0f, 1f)]
    public float groundDecay;

    [SerializeField] private float dashVelocity;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;
    private Vector2 dashDirection;
    private bool isDashing;
    private bool canDash = true;


    private void Start()
    {
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDashing)
        {
            return;
        }
        CheckInput();
        HandleJump();
        CheckDash();
        if(stateComplete)
        {
            SelectState();
        }
        UpdateState();

        UpdateFallDetectorPosition();



    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
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
                UpdateDashing();
                break;
            case PlayerState.Attacking:

                break;

        }

    }

    private void SelectState()
    {
        stateComplete = false;

        if(grounded && !isDashing)
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
        else if(!grounded && !isDashing)
        { 
            state = PlayerState.Airborne;
            StartAirborne();
        } 
        else if (isDashing)
        {
            state = PlayerState.Dashing;
            StartDashing();
        }

    }

    private void UpdateIdle()
    {
        if (!grounded || xInput != 0)
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

    private void UpdateDashing()
    {
        if(!isDashing)
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

    private void StartDashing()
    {
        animator.Play("dash");
    }

    private void CheckInput()
    {
        xInput = Input.GetAxis("Horizontal");
        
    }

    private void HandleMovement()
    {
        if(Mathf.Abs(xInput) > 0)
        {

            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);

            DirectionInput();
        }
    }

    private void CheckDash()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash == true)
        {

            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashVelocity, 0f);
        SelectState();
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        SelectState();
        canDash = true;
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

    private void UpdateFallDetectorPosition()
    {
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }
    }

}
