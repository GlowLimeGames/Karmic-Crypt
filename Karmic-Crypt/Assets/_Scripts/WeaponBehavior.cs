using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour {

    public bool thrown = false;
    public float throwSpeed;
    public bool held = false;

    public bool canJab;
    public bool canSwing;

    public int Health;
    public int Damage;

    public PlayerBehavior pb;
    Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 rotation = transform.localEulerAngles;
        rotation.x = 0;
        rotation.y = 0;
        if (held)
        {
            transform.localPosition = Vector3.zero;
            rotation.z = 0;
        }

        transform.localEulerAngles = rotation;
	}

    public void ThrowDirection(Vector2 dir)
    {
        if (!thrown)
        {
            StartCoroutine(Throwing(dir));
        }
    }

    IEnumerator Throwing(Vector2 dir)
    {
        thrown = true;
        held = false;

        Vector3 force = new Vector3(dir.x * throwSpeed, dir.y * throwSpeed, 0);
        
        rb.AddTorque(throwSpeed);
        rb.velocity = force;
        while (thrown)
        {
            
            yield return null;
        }

        thrown = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (thrown)
        {
            thrown = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            return;
        }
        else
        {
            if (thrown)
            {
                if (!collision.CompareTag("Weapon"))
                {
                    TakeDamage(2);
                }
            }
        }

        if (held)
        {
            if (pb.currentState == PlayerBehavior.CharacterState.ATTACKING)
            {
                if (collision.CompareTag("Enemy"))
                {
                    collision.GetComponent<EnemyBehavior>().Hit(Damage);
                    TakeDamage(1);
                }
            }
        }
    }

    void TakeDamage(int hp)
    {
        Health -= hp;
        if (Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
