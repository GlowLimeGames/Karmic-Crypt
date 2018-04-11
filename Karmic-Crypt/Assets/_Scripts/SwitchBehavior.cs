using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehavior : MonoBehaviour {

    public bool canThrownHit;

    public SpriteRenderer sprite;

    public List<DoorBehavior> Doors;

    bool hit;

    private void Update()
    {
        if (hit)
        {
            return;
        }

        if (Doors.Count < 0)
        {

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (hit)
        {
            return;
        }

        if (canThrownHit)
        {
            if (collision.gameObject.tag == "Thrown")
            {
                Open();
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Open();
            }
        }
    }

    void Open()
    {
        foreach (DoorBehavior door in Doors)
        {
            door.switchList.Remove(this);
        }

        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);

        hit = true;
    }
}
