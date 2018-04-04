using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : EnemyBehavior {

    public float chargeDistance = 5f;
    public float chargeSpeed = 5f;
    public float chargeDelay = 1f;
    public float crashDelay = 1f;
    bool charging = false;
    bool crashed = false;

    protected override void FixedUpdate()
    {
        if (target != null)
        {
            if (Vector3.Distance(target.transform.localPosition, transform.localPosition) <= chargeDistance
                && ((target.transform.localPosition.x - transform.localPosition.x < 0 && dir < 0)
                || (target.transform.localPosition.x - transform.localPosition.x > 0 && dir > 0))
                && ((target.transform.localPosition.y < transform.localPosition.y + transform.localScale.y / 2)
                && (target.transform.localPosition.y > transform.localPosition.y - transform.localScale.y / 2)))
            {
                if (!charging)
                {
                    StartCoroutine(Charging());
                }
            }
        }

        if (charging)
        {
            return;
        }

        base.FixedUpdate();
    }

    IEnumerator Charging()
    {
        charging = true;
        rb2d.AddForce(new Vector2(-100f * transform.localScale.x, 50f));

        yield return new WaitForSeconds(chargeDelay);

        crashed = false;
        while (!crashed)
        {
            yield return new WaitForFixedUpdate();
            rb2d.velocity = new Vector2(dir * chargeSpeed, rb2d.velocity.y);
        }

        rb2d.AddForce(new Vector2(-150f * transform.localScale.x, 50f));

        yield return new WaitForSeconds(crashDelay);

        Flip();
        charging = false;
    }


    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Player")
        {
            if (charging)
            {
                crashed = true;
            }
        }

        base.OnCollisionEnter2D(other);
    }
}
