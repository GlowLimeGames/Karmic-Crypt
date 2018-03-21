using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {


    protected PlayerBehavior target;
    
    public float sightDistance = 1f;
    public float speed = 2f;
    public float chaseSpeed = 3f;

    public float chaseDistance;
    public float patrolDistance;
    public float xMin;
    public float xMax;

    public bool chasing = false;
    
    Rigidbody2D rb2d;
    SpriteRenderer sprite;
    public int dir = 1;

    public int Health = 3;
	// Use this for initialization
	protected virtual void Start () {
        xMin = transform.position.x - xMin;
        xMax = transform.position.x + xMax;

        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
	}

    protected virtual void FixedUpdate()
    {
        /*
        Debug.DrawLine(sight.position, end , Color.red, 0f);
        //Debug.DrawRay(sight.position, sight.right * sightDistance * dir, Color.red, 0f);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.CompareTag("Player"))
            {
                target = hit.collider.GetComponent<PlayerBehavior>();
                Chase();
                Debug.Log("Hit");
            }
        }
        */

        if (transform.position.x >= xMax && dir == 1)
        {
            Flip();
        }
        else if (transform.position.x <= xMin && dir == -1)
        {
            Flip();
        }

        if (!chasing)
        {
            rb2d.velocity = new Vector2(dir * speed, rb2d.velocity.y);
        }
        else
        {
            rb2d.velocity = new Vector2(dir * chaseSpeed, rb2d.velocity.y);
        }
    }

    void Flip()
    {
        dir *= -1;

        Vector3 scale = transform.localScale;
        scale.x *= -1;

        transform.localScale = scale;

        if (chasing)
        {
            chasing = false;
            target = null;
            xMin = transform.position.x - patrolDistance;
            xMax = transform.position.x + patrolDistance;
        }
    }

    void Chase()
    {
        if (dir == 1)
        {
            xMax = target.transform.position.x + chaseDistance;
        }
        else
        {
            xMin = target.transform.position.x - chaseDistance;
        }
    }

    public void Hit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            StartCoroutine(GetHit());
        }
    }

    IEnumerator GetHit()
    {
        float count = 0;
        float invulCount = 1f;

        //Some knockback here
        while (count < invulCount)
        {
            count += Time.deltaTime;
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        sprite.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!chasing)
            {
                target = other.GetComponent<PlayerBehavior>();
                Chase();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerBehavior>().Hit();
        }
    }
}
