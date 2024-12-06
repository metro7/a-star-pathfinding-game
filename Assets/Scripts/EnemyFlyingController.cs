using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingController : MonoBehaviour
{
    enum State { Idle, Flying, Death }

    State state;
    bool stateComplete;
    private bool detectedPlayer = false;

    private bool isAlive = true;
    public Animator animator;
    public GameObject player;
    public bool flip;
    public float detectionRadius;
    public LayerMask playerMask;


    private void Update()
    {
        Vector3 scale = transform.localScale;

        if (player.transform.position.x > transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x) * (flip ? -1 : 1);
        }
        else
        {
            scale.x = Mathf.Abs(scale.x) * -1 * (flip ? -1 : 1);
        }

        transform.localScale = scale;

        DetectPlayer();
        if (stateComplete)
        {
            SelectState();
        }
        UpdateState();
    }

    private void UpdateState()
    {
        switch (state)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Flying:
                UpdateFlying();
                break;
            case State.Death:
                UpdateDeath();
                break;
        }
    }
    
    private void SelectState()
    {
        stateComplete = false;

        if(!detectedPlayer && isAlive)
        {
            state = State.Idle;
            StartIdle();
        } 
        else if(detectedPlayer && isAlive)
        {
            state = State.Flying;
            StartFlying();
        }
        else if(!isAlive)
        {
            state = State.Death;
            StartDeath();
        }
    }

    private void StartIdle()
    {
        animator.Play("idle");
    }

    private void StartFlying()
    {
        animator.Play("fly");
    }

    private void StartDeath()
    {
        animator.Play("death");
    }

    private void UpdateIdle()
    {
        if(detectedPlayer)
        {
            stateComplete = true;
        }
    }

    private void UpdateFlying()
    {
        if(!isAlive)
        {
            stateComplete = true;
        }
    }

    private void UpdateDeath()
    {

    }

    public void TakeHit()
    {
        this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        isAlive = false;
        Destroy(gameObject, 1f);
    }

    private void DetectPlayer()
    {
        
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerMask);
        if (playerCollider != null)
        {
            detectedPlayer = true;
            this.gameObject.GetComponent<AIDestinationSetter>().enabled = true;
        }
    }
}
