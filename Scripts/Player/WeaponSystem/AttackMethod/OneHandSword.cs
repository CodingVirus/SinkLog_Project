using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneHandSword : Stat
{
    public Transform myAttackPos;
    public LayerMask enemyLayer;
    public float attackRanage = 0.5f;


    void Start()
    {

    }

    void Update()
    {
        if (gameObject.GetComponentInParent<WeaponManager>())
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }
        }
    }

    void Attack()
    {        
        myAnim.SetTrigger("Attack");

        Collider2D[] hit = Physics2D.OverlapCircleAll(myAttackPos.position, attackRanage, enemyLayer);

        foreach (Collider2D enemy in hit)
        {
            Debug.Log("hit Enemy");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (myAttackPos == null) return;

        Gizmos.DrawWireSphere(myAttackPos.position, attackRanage);
    }
}
