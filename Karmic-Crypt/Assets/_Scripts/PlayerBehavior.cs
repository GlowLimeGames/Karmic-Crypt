using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    public Transform weaponSlot;
    public WeaponBehavior equipped;

    public float invulCount;

    public float jabSpeed = 0.5f;
    public float swingSpeed = 1f;

    bool equipping = false;
    SpriteRenderer sprite;
    Animator anim;
    Rigidbody2D rb;

    public enum CharacterState
    {
        IDLE,
        ATTACKING,
        JUMPING,
        HIT
    }

    public CharacterState currentState;
	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (equipped && !equipping)
        {
            if (Input.GetKey(KeyCode.K))
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                Throw(new Vector2(horizontal, vertical));
            }
        }

        if (equipped && !equipping)
        {
            if (Input.GetKey(KeyCode.J))
            {
                if (equipped.canJab)
                {
                    Jab();
                }else if (equipped.canSwing)
                {
                    Swing();
                }
            }
        }


    }

    public void Equip(WeaponBehavior weapon)
    {
        if (!equipping)
        {
            StartCoroutine(Equipping(weapon));
        }
    }

    IEnumerator Equipping(WeaponBehavior weapon)
    {
        if (equipped != null)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Throw(new Vector2(horizontal, vertical));
        }
        equipped = weapon;
        if (equipped.transform.parent != null)
        {
            equipped.transform.parent.GetComponent<MovingPlatform>().otherObjs.Remove(equipped.transform);
        }
        equipped.thrown = false;
        equipped.transform.parent = weaponSlot;
        equipped.transform.localPosition = Vector3.zero;
        equipped.pb = this;
        equipped.held = true;

        float delay = 1f;
        float count = 0;
        equipping = true;
        while (count < delay)
        {
            count += Time.deltaTime;
            yield return null;
        }
        equipping = false;
    }

    public void Throw(Vector2 direction)
    {
        equipped.transform.parent = null;
        if (direction.x == 0 && direction.y == 0)
        {
            direction.x = Mathf.FloorToInt(transform.localScale.x);
        }
        equipped.ThrowDirection(direction);
        equipped = null;
    }

    public void Swing()
    {
        if (currentState == CharacterState.ATTACKING)
        {
            return;
        }

        anim.SetTrigger("Swing");
        StartCoroutine(AttackDelay(swingSpeed));
    }

    public void Jab()
    {
        if (currentState == CharacterState.ATTACKING)
        {
            return;
        }

        anim.SetTrigger("Jab");
        StartCoroutine(AttackDelay(jabSpeed));
    }

    IEnumerator AttackDelay(float attackTime)
    {
        float timer = 0;
        currentState = CharacterState.ATTACKING;
        while (timer < attackTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        currentState = CharacterState.IDLE;
    }

    public void Hit()
    {
        StartCoroutine(GetHit());
    }

    IEnumerator GetHit()
    {
        float count = 0;
        currentState = CharacterState.HIT;

        rb.AddForce(new Vector2(-100f * transform.localScale.x, 50f));
        //Some knockback here
        while (count < invulCount)
        {
            count += Time.deltaTime;
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        sprite.enabled = true;
        currentState = CharacterState.IDLE;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Thrown"))
        {
            Hit();
        }
    }
}
