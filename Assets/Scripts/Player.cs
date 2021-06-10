using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Config
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    //State
    bool isAlive = true;

    // Components
    Rigidbody2D rb;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeet;
    float startingGravity;

    string GROUND_LAYER = "Ground";
    string CLIMBING_LAYER = "Climbing";

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
        Run();
        Jump();
        FlipSprite();
        ClimbLadder();
    }

    private void Run()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        Vector2 playerVelocity = new Vector2(horizontalMove, rb.velocity.y);
        rb.velocity = playerVelocity;
        myAnimator.SetBool("Running", Mathf.Abs(horizontalMove) > Mathf.Epsilon ? true : false);
    }

    private void Jump()
    {
        if(!myFeet.IsTouchingLayers(LayerMask.GetMask(GROUND_LAYER))){ return; }

        if (Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(rb.velocity.x, jumpSpeed);
            rb.velocity += jumpVelocityToAdd;
        }
    }

    private void ClimbLadder()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask(CLIMBING_LAYER))) {
            myAnimator.SetBool("Climbing", false);
            rb.gravityScale = startingGravity;
            return;
        }
        rb.gravityScale = 0f;
        float verticalMove = Input.GetAxisRaw("Vertical") * climbSpeed;
        Vector2 climbVelocity = new Vector2(rb.velocity.x, verticalMove);
        rb.velocity = climbVelocity;
        myAnimator.SetBool("Climbing", Mathf.Abs(verticalMove) > Mathf.Epsilon ? true : false);
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
