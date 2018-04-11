using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPlatform : MonoBehaviour {

    PlatformEffector2D effector;

    Rigidbody2D player;
    public LayerMask defaultmask;
    public LayerMask dropMask;
	// Use this for initialization
	void Start () {
        effector = GetComponent<PlatformEffector2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.S))
        {
            if (player != null)
            {
                if (player.transform.position.y > transform.position.y + transform.localScale.y)
                {
                    effector.colliderMask = dropMask;
                    gameObject.layer = 9;
                    player.velocity = new Vector2(player.velocity.x, -5f);
                    StartCoroutine(DropWait());
                }
            }
        }
	}

    IEnumerator DropWait()
    {
        player = null;
        yield return new WaitForSeconds(0.15f);
        effector.colliderMask = defaultmask;
        gameObject.layer = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (player != null)
        {
            player = null;
        }
    }
}
