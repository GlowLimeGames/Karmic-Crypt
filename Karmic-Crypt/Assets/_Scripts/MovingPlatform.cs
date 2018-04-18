using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public Transform targetPos;

    public float speed;

    float percent = 0f;
    Vector2 startPos;
    Vector2 endPos;

    int dir = 1;

    public List<Transform> otherObjs = new List<Transform>();
    private void Start()
    {
        startPos = transform.position;
        endPos = targetPos.position;
    }

    // Update is called once per frame
    void Update () {
        percent += dir * Time.deltaTime / speed;

        transform.localPosition = Vector2.Lerp(startPos, endPos, percent);
		
        if (percent > 1f)
        {
            dir = -1;
        }else if (percent < 0)
        {
            dir = 1;
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.parent != null)
        {
            return;
        }

        if (!otherObjs.Contains(collision.transform))
        {
            if (collision.transform.position.y > transform.position.y)
            {
                otherObjs.Add(collision.transform);
                collision.transform.parent = transform;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (otherObjs.Contains(collision.transform))
        {
            otherObjs.Remove(collision.transform);
            collision.transform.parent = null;
        }
    }
}
