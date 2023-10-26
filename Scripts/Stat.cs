using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public interface IDamage
{
    void OnDamage(float dmg);
}

public struct Stats
{
    public float MaxHp;
    public float CurHp;
    public float AttackMinDamage;
    public float AttackMaxDamage;
    public float DefenesPoint;
    public float CriticalRate;
    public float CriticalDamage;
    public float AttackSpeed;
    public float MoveSpeed;
    public float DodgeRate;
}

public class Stat : CharacterProperty2D, IDamage
{
    public UnityEvent Dead;

    public float MHP = 0;
    public float CHP = 0;
    public float AttMinDmg = 0;
    public float AttMaxDmg = 0;
    public float DEF = 0;
    public float CriRate = 0;
    public float CriDmg = 0;
    public float AttSpeed = 0;
    public float MSpeed = 0;
    public float Dodge = 0;

    // Player Stat
    protected float basicCriDmg = 120.0f;
    protected float basicCriRate = 5.0f;
    protected float basicMinAD = 1.0f;
    protected float basicMaxAD = 3.0f;
    protected float basicMoveSpeed = 3.0f;

    // WeaponDataSO Stat

    // Total Stat Amount
    /*protected float totalAttMinDmg;
    protected float totalAttMaxDmg;
    protected float totalDEF;
    protected float totalCriRate;
    protected float totalCriDmg;
    protected float totalAttSpeed;
    protected float totalMSpeed;
    protected float totalDodge;*/

    /*protected void Initialize()
    {
        MHP = stat.MaxHp;
        curHP = MHP;
        CHP = curHP;
        AttMinDmg = stat.AttackMinDamage;
        AttMaxDmg = stat.AttackMaxDamage;
        DEF = stat.DefenesPoint;
        CriRate = stat.CriticalRate;
        CriDmg = stat.CriticalDamage;
        AttSpeed = stat.AttackSpeed;
        MSpeed = stat.MoveSpeed;
        Dodge = stat.DodgeRate;
    }*/

    public void OnAttack()
    {
        IDamage damage = GetComponent<IDamage>();
        if (damage != null) damage.OnDamage(Random.Range(AttMinDmg, AttMaxDmg));
    }

    public void OnDamage(float dmg)
    {
        curHP = CHP;
        CHP -= Random.Range(WeaponManager.Instance.AttMinDmg, WeaponManager.Instance.AttMaxDmg);
        if (CHP > 0.0f)
        {
            myAnim.SetTrigger("OnDamage");
        }
        else
        {
            if (Dead != null)
            {
                Dead?.Invoke();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}


