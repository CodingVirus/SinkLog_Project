using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask crashMask;
    public float speed;
    public LayerMask groundMask;
    public float lifeTime;
    public float distance;
    [SerializeField] private float minDmg;
    [SerializeField] private float maxDmg;

    public GameObject destroyEffect;

    [SerializeField] ItemDataSO weaponData;

    void Start()
    {
        Invoke("DestroyBullet", lifeTime);
        minDmg = weaponData.attackMinDamage;
        maxDmg = weaponData.attackMaxDamage;
    }

    
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, crashMask);
        RaycastHit2D hitground = Physics2D.Raycast(transform.position, transform.up, distance, groundMask);
        if (hitInfo.collider != null)
        {
            hitInfo.collider.GetComponent<NormalMonster>().OnDamage(Random.Range(minDmg, maxDmg));
            DestroyBullet();
        }
        if (hitground.collider != null)
        {
            DestroyBullet();
        }
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    void DestroyBullet()
    {
        /*Instantiate(destroyEffect, transform.position, Quaternion.identity);*/
        Destroy(gameObject);
    }
}
