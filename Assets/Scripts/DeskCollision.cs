using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskCollision : MonoBehaviour
{
    private PlayerAttributes player;


    [SerializeField] public ContactPoint2D[] points;
    [SerializeField] public Collision2D col;
    public int test;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
    }

    private void Update()
    {
        if (col != null)
        {
            points = col.contacts;
        }       
        else
        {
            points = null; 
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.HasTag("Player"))
        {
            return;
        }
        EnableCollisionWithPlayer(collision);
        col = collision;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.HasTag("Player"))
        {
            return;
        }
        EnableCollisionWithPlayer(collision);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        DisableCollisionWithPlayer(collision);
    }

    void EnableCollisionWithPlayer(Collision2D collision)
    {
        if (collision.gameObject.HasTag("Player"))
        {
            return;
        }
        bool collisionIsHorizontal = false;
        bool collisionIsVertical = false;

        foreach(ContactPoint2D point in collision.contacts)
        {
            if (Mathf.Abs(point.normal.x) > 0.5f)
            {
                collisionIsHorizontal = true;
            }
            if (Mathf.Abs(point.normal.y) > 0.5f)
            {
                collisionIsVertical = true;
            }
        }
        if (player.canMoveHorizontal && collisionIsHorizontal)
        {
            this.transform.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        else if (player.canMoveVertical && collisionIsVertical)
        {
            this.transform.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    void DisableCollisionWithPlayer(Collision2D collision) {
        if (collision.gameObject.HasTag("Player"))
        {
            return;
        }
        this.transform.gameObject.layer = LayerMask.NameToLayer("Player Noncollision");
    }
}
