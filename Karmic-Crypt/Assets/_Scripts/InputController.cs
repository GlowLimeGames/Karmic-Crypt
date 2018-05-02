using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public float movementSpeed;
    public float jumpForce;
    public Transform[] groundPoints;
    public float groundRadius;
    public LayerMask isGround;

    float jumpTimer;
    float jumpLimit = 0.25f;

    Rigidbody2D rb;
    bool falling = false;
    bool canJump = true;
    bool grounded = true;
    bool canEquip = false;
    bool facingRight = true;
    bool flipping = false;

    WeaponBehavior hovering;
    PlayerBehavior pb;
    Animator anim;
	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody2D>();
        pb = GetComponent<PlayerBehavior>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        if (grounded)
        {
            Move(horizontal);
        }

        grounded = IsGrounded();
        canEquip = OverWeapon();

        if (canEquip)
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (hovering != null)
                {
                    pb.Equip(hovering);
                }
            }
        }

        if (grounded)
        {
            falling = false;
        }

        if (Time.time - jumpTimer < jumpLimit)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (grounded)
            {
                jumpTimer = Time.time;
            }

            if (canJump)
            {
                Jumping();
            }
        }
        else
        {
            falling = true;
        }
    }

    void Move(float horizontal)
    {
        if (!facingRight && horizontal > 0 || facingRight && horizontal < 0)
        {
            if (!flipping)
            {
                anim.SetTrigger("Flip");
                StartCoroutine(Turning());
            }
        }

        rb.velocity = new Vector2(horizontal * movementSpeed, rb.velocity.y);
    }

    IEnumerator Turning()
    {
        flipping = true;

        yield return new WaitForSeconds(0.625f);

        flipping = false;

        facingRight = !facingRight;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }

    bool IsGrounded()
    {

        // After landing, the Y velocity fluctuates between a bunch of really low numbers for ~1s.
        // Safer to check if it's lower than 0.1 than 0.0
        if (rb.velocity.y <= 0.1)
        {
            // for every ground point a new collider is made. 
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, isGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    //returns true if the collider is touching something other then the player. 
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
            //returns false if the velocity of the player is greater then 0 . 
        }
        return false;
    }

    bool OverWeapon()
    {
        if (rb.velocity.y <= 0.1)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius);
                for (int i = 0; i < colliders.Length; i++)
                {
                    //returns true if the collider is touching something other then the player. 
                    if (colliders[i].gameObject != gameObject)
                    {
                        if (colliders[i].GetComponent<WeaponBehavior>() != null)
                        {
                            hovering = colliders[i].GetComponent<WeaponBehavior>();
                            return true;
                        }
                    }
                }
            }
        }
        hovering = null;
        return false;
    }

    void Jumping()
    {
        if (!falling)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
