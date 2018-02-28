using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour {

    public bool thrown = false;
    public float throwSpeed;
    public bool held = false;

    public bool canJab;
    public bool canSwing;

    public int Damage;

    public PlayerBehavior pb;
    Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (held)
        {
            transform.localPosition = Vector3.zero;
        }
	}

    public void ThrowStraight(int dir)
    {
        if (!thrown)
        {
            StartCoroutine(Straight(dir));
        }
    }

    IEnumerator Straight(int dir)
    {
        thrown = true;
        held = false;

        while (thrown)
        {
            rb.velocity = transform.right * throwSpeed * dir;
            yield return null;
        }

        thrown = false;
        Break();
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
                    Break();
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
                }
            }
        }
    }

    void Break()
    {
        Destroy(this.gameObject);
    }
}
