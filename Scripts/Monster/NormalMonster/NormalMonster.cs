using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class NormalMonster : Stat, IDamage, IFindEnemy, ILostEnemy
{
    WeaponManager wm;
    public enum State
    {
        Create, Normal, Roaming, Battle, Dead
    }

    public UnityEvent AttackPlayer;
    public float AttackRange = 1.0f;
    public State myState = State.Create;
    public float moveSpeed = 1.0f;
    Vector3 myDir = Vector3.right;
    public LayerMask groundMask;
    public Transform myTarget = null;
    float originSpeed = 0.0f;
    public LayerMask Player;
    public GameObject Effect;
    SpriteRenderer EffectRend;
    public float AttackingMoveSpeed = 6.0f;
    float rightposX = 0.0f;
    bool AttackP = false;

    private void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case State.Create:
                break;
            case State.Normal:
                StartCoroutine(OnChangeRoaming());
                break;
            case State.Roaming:
                myAnim.SetBool("Move", true);
                break;
            case State.Battle:
                Effect.SetActive(true);
                break;
        }
    }

    private void StateProcess()
    {
        switch (myState)
        {
            case State.Create:
                break;
            case State.Normal:
                break;
            case State.Roaming:
                Effect.SetActive(false);
                moveSpeed = originSpeed;
                Vector3 pos = transform.position + myDir * moveSpeed;
                BoxCollider2D capsule = myCollider as BoxCollider2D;
                if (capsule != null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, capsule.size.y * 0.5f + 0.2f, groundMask);
                    if (hit.transform == null)
                    {
                        myDir *= -1;
                        myRender.flipX = !myRender.flipX;
                    }
                }
                transform.Translate(myDir * Time.deltaTime * moveSpeed, Space.World);
                break;
            case State.Battle:
                StartCoroutine(monsterBattle());
                Vector3 pos2 = transform.position + myDir * 0.5f;
                BoxCollider2D capsule2 = myCollider as BoxCollider2D;
                if (capsule2 != null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(pos2, Vector2.down, capsule2.size.y * 0.5f + 0.2f, groundMask);
                    Debug.DrawRay(pos2, Vector2.down, Color.red, capsule2.size.y * 0.5f + 0.2f);
                    if (hit.transform == null)
                    {
                        moveSpeed = 0;
                    }
                    else
                    {
                        moveSpeed = AttackingMoveSpeed;
                    }
                }
                break;
        }
    }

    public void OnFindEnemy(Transform target)
    {
        if(myState != State.Dead)
        {
            myTarget = target;
            ChangeState(State.Battle);
        }
    }

    public void OnLostEnemy()
    {
        if (myState != State.Dead)
        {
            StopAllCoroutines();
            ChangeState(State.Normal);
        }
    }

    void Start()
    {
        ChangeState(State.Normal);
        MHP = 30.0f;
        CHP = MHP;
        curHP = CHP;
        rightposX = Effect.transform.localPosition.x;
        Effect.SetActive(false);
        originSpeed = moveSpeed;
        EffectRend = Effect.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        StateProcess();
    }
    void Damage(float dmg)
    {
        CHP -= Random.Range(WeaponManager.Instance.AttMinDmg, WeaponManager.Instance.AttMaxDmg);
        if(CHP <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
    public void OnDead()
    {
        StopAllCoroutines();
        ChangeState(State.Dead);
        Destroy(gameObject);
    }

    IEnumerator OnChangeRoaming()
    {
        ChangeState(State.Roaming);
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator BattlePattern()
    {
        StopAllCoroutines();
        yield return new WaitForSeconds(2.0f);
        ChangeState(State.Roaming);
    }

    IEnumerator next()
    {
        yield return new WaitForSeconds(3.0f);
        ChangeState(State.Roaming);
        StopCoroutine(next());
    }
    

    IEnumerator monsterBattle()
    {
        Effect.SetActive(true);
        if (transform.position.x > myTarget.position.x)
        {
            myDir = Vector2.left;
            myRender.flipX = true;
            Effect.transform.localPosition = new Vector2(-rightposX, Effect.transform.localPosition.y);
            EffectRend.flipX = true;
        }
        else
        {
            myDir = Vector2.right;
            myRender.flipX = false;
            Effect.transform.localPosition = new Vector2(rightposX, Effect.transform.localPosition.y);
            EffectRend.flipX = false;
        }
        if (AttackP)
        {
            moveSpeed = 0;
            yield return new WaitForSeconds(2.0f);
            moveSpeed = AttackingMoveSpeed;
            AttackP = false;
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
            transform.Translate(myDir * Time.deltaTime * moveSpeed, Space.World);
            yield return null;
        }
        
        /*if (AttackRange < Mathf.Abs(transform.position.x - myTarget.position.x))
        {
            transform.Translate(myDir * Time.deltaTime * moveSpeed, Space.World);
        }
        else
        {
            if (AttackRange > Mathf.Abs(transform.position.y - myTarget.position.y))
            {
                myAnim.SetTrigger("Attack");
            }
        }*/
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((Player & 1 << collision.gameObject.layer) != 0)
        {
            Debug.Log("player 충돌 감지");
            AttackPlayer?.Invoke();
            AttackP = true;
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
