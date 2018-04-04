using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingEnemy : EnemyBehavior {

    public GameObject throwingObj;

    public float throwSpeed = 2f;
    public float fallSpeed = 1.5f;

    public float throwDistance = 5f;
    public float throwHeight = 5f;
    float throwDelay = 1f;
    bool throwing = false;
    
    protected override void FixedUpdate()
    {
        if (target != null)
        {
            if (Vector3.Distance(target.transform.localPosition, transform.localPosition) <= throwDistance
                && ((target.transform.localPosition.x - transform.localPosition.x < 0 && dir < 0) 
                || (target.transform.localPosition.x - transform.localPosition.x > 0 && dir > 0)))
            {
                if (!throwing)
                {
                    StartCoroutine(Throwing());
                }
            }
        }

        if (throwing)
        {
            return;
        }

        base.FixedUpdate();
    }

    IEnumerator Throwing()
    {
        throwing = true;

        yield return new WaitForSeconds(throwDelay);

        GameObject thrown = Instantiate(throwingObj, transform.position, Quaternion.identity);

        Rigidbody2D throwRB = thrown.GetComponent<Rigidbody2D>();

        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position;

        endPos.x += Random.Range(-1f, 1f) + transform.localScale.x * throwDistance;

        throwRB.velocity = new Vector2(endPos.x / 2, throwHeight);
        throwRB.AddTorque(throwHeight * 6f);
        Vector3 targetDir = thrown.transform.position - new Vector3(endPos.x / 2, throwHeight, thrown.transform.position.z);
        thrown.transform.right = targetDir;

        yield return new WaitForSeconds(throwDelay);
        /*
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position;

        endPos.x += transform.localScale.x * throwDistance / 4;
        endPos.y += throwHeight;
        
        float percentComplete = 0f;
        GameObject thrown = Instantiate(throwingObj, transform.position, Quaternion.identity);
        
        Vector3 targetDir = endPos - thrown.transform.position;
        while (percentComplete <= 1f)
        {
            percentComplete += Time.deltaTime * throwSpeed;
            thrown.transform.position = Vector3.Lerp(startPos, endPos, percentComplete);

            targetDir = thrown.transform.position - startPos;
            thrown.transform.right = targetDir;

            yield return null;
        }

        startPos = thrown.transform.position;
        endPos.x += Random.Range(-1f, 1f) + transform.localScale.x * throwDistance / 4;
        endPos.y -= throwHeight * 1.5f;

        percentComplete = 0f;
        while (percentComplete <= 1f)
        {
            percentComplete += Time.deltaTime * fallSpeed;
            thrown.transform.position = Vector3.Lerp(startPos, endPos, percentComplete);

            targetDir = thrown.transform.position - startPos;
            thrown.transform.right = targetDir;
            yield return null;
        }
        */

        throwing = false;
    }
}
