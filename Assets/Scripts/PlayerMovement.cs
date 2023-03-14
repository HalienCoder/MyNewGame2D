using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float horizontal;
    
    public float playerSpeed = 5;
    public float jumpForce = 2;
    public float raycastLength = 4;
    
    public bool isGrounded;
    public LayerMask groundLayerMask;
    public Transform respawnPoint;

    private SpriteRenderer spriteRenderer;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        respawnPoint.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(horizontal * playerSpeed, rb.velocity.y);
        horizontal = Input.GetAxis("Horizontal");
        // this sets a raycast, it makes sure that u can jump only when the greenline is touching the ground.
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        // this makes sure the animations are changed acc to the movement
        if(rb.velocity.x != 0)
        {
            anim.SetBool("isMoving" , true);
        }
        else
        {
            anim.SetBool("isMoving" , false);
        }
        // this flips the axis of the sprite, so it when moves backwards, it turns backwards.
        if (horizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontal > 0)
        {
            spriteRenderer.flipX = false;
        }

        // to check wether the player is on the ground
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, raycastLength, groundLayerMask);
        Debug.DrawRay(transform.position, Vector3.down * raycastLength, Color.green);

        //to animate jumping
        anim.SetBool("isGrounded", isGrounded);

        
    }
    //to collect coins
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coin")
        {
            Destroy(other.gameObject);
        }

        if(other.tag == "Respawn")
        {
            Respawn();
        }
    }
    void Respawn()
    {
        transform.position = respawnPoint.position;
    }
}