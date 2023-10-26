using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Boss : NormalMonster, IDamage
{

    public List<Transform> SpotList;
    GameObject obj = null;
    GameObject LaserObj = null;
    Vector2 StartPos = Vector2.zero;
    public Transform ShootPos;
    public Transform LaserPos;
    bool shooting = false;
    bool right = true;
    int pattern = 0;
    int spot = 0;
    float ShootPosX = 0;

    [SerializeField] private UIResultScreen uiResultScreen;

    [SerializeField] private TMPro.TMP_Text MAXHP;
    [SerializeField] private TMPro.TMP_Text CURHP;

    void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case State.Create:
                break;
            case State.Normal:
                ChangeState(State.Roaming);
                break;
            case State.Roaming:
                spot = Random.Range(0, SpotList.Count);
                MoveToPos(SpotList[spot].position); //spotlist 랜덤 위치로이동
                break;
            case State.Battle:
                break;
            case State.Dead:
                uiResultScreen.StartShowResult(true, 1100);
                //OnDead();
                break;
        }
    }

    void StateProcess()
    {
        switch (myState)
        {
            case State.Create:
                break;
            case State.Normal:
                break;
            case State.Roaming:
                break;
            case State.Battle:
                pattern = Random.Range(0, 2);
                if(pattern == 0)
                {
                    if (shooting == false)
                    {
                        myAnim.SetTrigger("Shoot");
                    }
                }
                else
                {
                    myAnim.SetTrigger("Laser");
                }
                StartCoroutine(Waiting(3.0f));
                break;
            case State.Dead:
                
                break;
        }
    }

    private void Awake()
    {
        stat.MaxHp = MHP;
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(State.Normal);
        ShootPosX = ShootPos.position.x;
        MAXHP.text = MHP.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        CHP = (CHP < 0) ? 0 : CHP ;
        CURHP.text = $"{(int)CHP}";
        StateProcess();
        flipX();
    }

    IEnumerator Waiting(float t)
    {
        yield return new WaitForSeconds(t);
        ChangeState(State.Normal);
    }

    public void ShootProjectile()
    {
            obj = Instantiate(Resources.Load("Projectile") as GameObject, ShootPos.position, Quaternion.identity, ShootPos);
            obj.GetComponent<Projectiles>().Initialize(this);
            shooting = true;
    }

    void Laser()
    {
        LaserObj = Instantiate(Resources.Load("Laser") as GameObject, LaserPos);
    }

    void MoveToPos(Vector2 pos)
    {
        StopAllCoroutines();
        StartCoroutine(MovingToPos(pos, null));
    }
    void MoveToPos(Vector2 pos, UnityAction done)
    {
        StopAllCoroutines();
        StartCoroutine(MovingToPos(pos, done));
    }

    IEnumerator MovingToPos(Vector2 pos, UnityAction done)
    {
        myAnim.SetBool("Shooting", true);
        myAnim.SetBool("Move", true);
        Vector2 dir = pos - (Vector2)transform.position;
        float dist = dir.magnitude;
        dir.Normalize();
        while (dist > 0.0f)
        {
            float delta = moveSpeed* 5 * Time.deltaTime;
            if (delta > dist)
            {
                delta = dist;
            }
            dist -= delta;
            transform.Translate(dir * delta, Space.World);
            yield return null;
        }
        if (transform.position.x > 0)
        {
            right = true;
        }
        else
        {
            right = false;
        }
        myAnim.SetBool("Move", false);
        ChangeState(State.Battle);
        myAnim.SetBool("Shooting", false);
        done?.Invoke();
    }

    public void OnDead()
    {
        StopAllCoroutines();
        ChangeState(State.Dead);
        myAnim.SetTrigger("Dead");
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

    public new void OnDamage(float dmg)
    {
        curHP = CHP;
        curHP -= Random.Range(WeaponManager.Instance.AttMinDmg, WeaponManager.Instance.AttMaxDmg);
        CURHP.text = curHP.ToString();
        if (curHP <= 0)
        {
            StopAllCoroutines();
            ChangeState(State.Dead);
            myAnim.SetTrigger("--");
        }
    }

    public void Block()
    {
        myAnim.SetBool("Shooting", true);
    }
    void flipX()
    {
        if (right)
        {
            myRender.flipX = true;
            ShootPos.localPosition = new Vector2(-ShootPosX, ShootPos.localPosition.y);
        }
        else
        {
            myRender.flipX = false;
            ShootPos.localPosition = new Vector2(ShootPosX, ShootPos.localPosition.y);
        }
    }
}
