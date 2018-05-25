using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float movePower = 5f;
    public float jumpPower = 4f;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !anim.GetBool("IsJumping"))
        {
            isJumping = true;
            anim.SetTrigger("DoJump");
            anim.SetBool("IsJumping", true);
        }    
    }

    void FixedUpdate()
    {
        Move();
        Jump();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        float mover = Input.GetAxisRaw("Horizontal");

        if (mover < 0)
        {
            moveVelocity = Vector3.left;
            sr.flipX = true;
        }
        else if (mover > 0)
        {
            moveVelocity = Vector3.right;
            sr.flipX = false;
        }

        transform.position += moveVelocity * movePower * Time.fixedDeltaTime;
        anim.SetFloat("Speed", Mathf.Abs(mover));
    }

    void Jump()
    {
        if (!isJumping)
            return;

        rb.velocity = Vector2.zero;
        Vector2 jumpVelocity = new Vector2(0f, jumpPower);
        rb.AddForce(jumpVelocity, ForceMode2D.Impulse);
        isJumping = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == 8 || collision.gameObject.layer == 9) && rb.velocity.y <= 0)
            anim.SetBool("IsJumping", false);

        if(collision.gameObject.layer == 9 && rb.velocity.y <= 0)
        {
            BlockStatus block = collision.GetComponent<BlockStatus>();

            switch (block.type)
            {
                case "Up":
                    Vector2 v = new Vector2(0, block.value);
                    rb.velocity = Vector2.zero;
                    rb.AddForce(v, ForceMode2D.Impulse);
                    break;
            }
        }
    }

}
