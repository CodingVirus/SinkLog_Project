using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Laserobj : CharacterProperty2D
{
    Collider2D myCollider2D;
    public LayerMask Player;
    public UnityEvent AttackPlayer;
    // Start is called before the first frame update
    void Start()
    {
        myCollider2D = GetComponent<Collider2D>();
        ColliderOff();
        Vector2 Position = transform.TransformPoint(transform.position); //로컬 포지션을 월드 포지션 값으로 바꿈.
        if (Position.x > 0)
        {
            myRender.flipX = true;
            myCollider2D.offset = new Vector2(-myCollider2D.offset.x, myCollider2D.offset.y);
        }
        else
        {
            myRender.flipX=false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ColliderOn()
    {
        myCollider2D.enabled = true;
    }

    void ColliderOff()
    {
        myCollider2D.enabled = false;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((Player & 1 << collision.gameObject.layer) != 0)
        {
            AttackPlayer?.Invoke();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((Player & 1 << collision.gameObject.layer) != 0)
        {
            AttackPlayer?.Invoke();
        }
    }
}
