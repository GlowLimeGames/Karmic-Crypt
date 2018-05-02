using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorBehavior : MonoBehaviour {

    public bool isLocked;

    public bool switchLock;
    public List<SwitchBehavior> switchList = new List<SwitchBehavior>();
    public bool enemyLock;
    public List<EnemyBehavior> enemyList = new List<EnemyBehavior>();

    public int SceneToGo;

    private void Start()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = isLocked;
    }

    private void Update()
    {
        if (!isLocked)
        {
            return;
        }

        if (!enemyLock && !switchLock && isLocked)
        {
            isLocked = false;
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }

        if (enemyList.Count <= 0 && enemyLock)
        {
            enemyLock = false;
        }

        if (switchList.Count <= 0 && switchLock)
        {
            switchLock = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isLocked)
        {
            return;
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
        SceneManager.LoadScene(SceneToGo);
    }
}
