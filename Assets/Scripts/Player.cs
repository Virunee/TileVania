using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Config
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float jumpDuration = 0.2f;

    //State
    bool isAlive = true;
    bool isJumping = false;

    // Components
    Rigidbody2D rb;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeet;
    float startingGravity;

    string GROUND_LAYER = "Ground";
    string CLIMBING_LAYER = "Climbing";
    string ENEMY_LAYER = "Enemy";
    string HAZARDS_LAYER = "Hazards";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        startingGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive) { return; }
        Run();
        Jump();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    private void Run()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        Vector2 playerVelocity = new Vector2(horizontalMove, rb.velocity.y);
        rb.velocity = playerVelocity;
        myAnimator.SetBool("Running", Mathf.Abs(horizontalMove) > Mathf.Epsilon ? true : false);
    }

    private void Die()
    {
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask(ENEMY_LAYER, HAZARDS_LAYER))) { return; }
        isAlive = false;
        myAnimator.SetBool("Dead", !isAlive);
        rb.velocity = new Vector2(25f, 25f);
        runSpeed = 0f;
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    private void Jump()
    {
        if((!myFeet.IsTouchingLayers(LayerMask.GetMask(GROUND_LAYER)) && !myFeet.IsTouchingLayers(LayerMask.GetMask(CLIMBING_LAYER))) || isJumping){ return; }

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            Vector2 jumpVelocityToAdd = new Vector2(rb.velocity.x, jumpSpeed);
            rb.velocity += jumpVelocityToAdd;
            StartCoroutine(resetJump());
        }
    }

    IEnumerator resetJump()
    {
        yield return new WaitForSeconds(jumpDuration);
        isJumping = false;
    }

    private void ClimbLadder()
    {
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask(CLIMBING_LAYER)) || isJumping) {
            myAnimator.SetBool("Climbing", false);
            rb.gravityScale = startingGravity;
            return;
        }
        
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask(CLIMBING_LAYER)) && !isJumping) {
            rb.gravityScale = 0f;
            float verticalMove = Input.GetAxisRaw("Vertical") * climbSpeed;
            Vector2 climbVelocity = new Vector2(rb.velocity.x, verticalMove);
            rb.velocity = climbVelocity;
            myAnimator.SetBool("Climbing", Mathf.Abs(verticalMove) > Mathf.Epsilon ? true : false);
        }
    }

    private void FlipSprite()
    {
        bool playerHasHoriztonalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        if(playerHasHoriztonalSpeed)
        {
            rb.transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }
}
