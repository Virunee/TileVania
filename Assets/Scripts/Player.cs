using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Config
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;

    //State
    bool isAlive = true;

    // Components
    Rigidbody2D rb;
    Animator myAnimator;
    CapsuleCollider2D collider;

    string GROUND_LAYER = "Ground";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        FlipSprite();
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
        if(!collider.IsTouchingLayers(LayerMask.GetMask(GROUND_LAYER))){ return; }

        if (Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(rb.velocity.x, jumpSpeed);
            rb.velocity += jumpVelocityToAdd;
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
