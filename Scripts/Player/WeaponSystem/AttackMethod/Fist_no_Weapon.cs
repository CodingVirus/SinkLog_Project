using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// if you don't have weapon, Equipped Fist 
public class Fist_no_Weapon : Stat, IDamage
{
    public LayerMask enemyLayer;
    public Transform myWeaponPos;

    BoxCollider2D myBoxCollider;
    WeaponManager wm;

    float timeUntillMelee;
    void Start()
    {
        myBoxCollider = GetComponent<BoxCollider2D>();
        wm = GameObject.Find("WeaponSlot").GetComponent<WeaponManager>();
        /*transform.gameObject.SetActive(false);*/
    }

    void Update()
    {
        if (timeUntillMelee <= 0f)
        {
            if (Input.GetMouseButtonDown(0))
            {

                OnAttack();
                myAnim.SetTrigger("Attack");
                timeUntillMelee = AttSpeed;
            }
            else
            {
                timeUntillMelee -= Time.deltaTime;
            }
        }
    }


    public new void OnAttack()
    {
        Collider[] list = Physics.OverlapSphere(myWeaponPos.position, 1.0f, enemyLayer);
        foreach (Collider col in list)
        {
            IDamage damage = col.GetComponent<IDamage>();
            if (damage != null) damage.OnDamage(Random.Range(AttMinDmg, AttMaxDmg));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer & enemyLayer) != 0)
        {
            IDamage damage = collision.GetComponent<IDamage>();
            if (damage != null) damage.OnDamage(Random.Range(AttMinDmg, AttMaxDmg));
        }
    }

    IEnumerator Waiting(float t)
    {
        yield return new WaitForSeconds(t);
    }
}
