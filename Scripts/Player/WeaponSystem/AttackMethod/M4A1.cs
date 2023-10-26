using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class M4A1 : MonoBehaviour
{
    public GameObject myAmmo;
    public Transform ShotPoint;

    [SerializeField] private AudioSource fireSound;

    private float timeBtwShots;
    public float startTimeBtwShots;
    public float attackDelay;
    public float timereset;

    void Start()
    {
        attackDelay = 5.0f;
        timereset = attackDelay;
    }

    void Update()
    {
        if (gameObject.GetComponentInParent<WeaponManager>())
        {
            if (timeBtwShots <= 0)
            {
                if (Input.GetMouseButton(0))
                {
                    attackDelay -= 0.1f;
                    if (attackDelay <= 0)
                    {
                        Instantiate(myAmmo, ShotPoint.position, transform.rotation);
                        fireSound.Play();
                        attackDelay = timereset;
                    }
                    timeBtwShots = startTimeBtwShots;
                }
                else
                {
                    timeBtwShots -= Time.deltaTime;
                }
            }
        }
    }
}
