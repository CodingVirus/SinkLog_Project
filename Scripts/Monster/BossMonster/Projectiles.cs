using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectiles : CharacterProperty2D
{
    public LayerMask Obstacle; //장애물 닿을시 삭제
    public LayerMask Player;
    public Transform Target;
    public NormalMonster enemy;
    public float PmoveSpeed = 5.0f;
    Vector2 Dir = Vector2.zero;
    public UnityEvent AttackPlayer;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(null);
        StartCoroutine(off());
        if (transform.position.x > 0)
        {
            myRender.flipX = false;
        }
        else
        {
            myRender.flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            transform.Translate(Dir * PmoveSpeed * Time.deltaTime);
        }
    }

    public void Initialize(NormalMonster enemy)
    {
        this.enemy = enemy;
        Target = this.enemy.myTarget;
        Dir = (Target.position - transform.position).normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((Obstacle & 1 << collision.gameObject.layer) != 0)
        {
            Destroy(gameObject);
        }
        if ((Player & 1 << collision.gameObject.layer) != 0)
        {
            AttackPlayer?.Invoke();
        }
    }

    IEnumerator off()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}

