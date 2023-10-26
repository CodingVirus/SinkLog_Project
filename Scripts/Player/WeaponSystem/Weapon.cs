using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private ItemDataSO weaponData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        WeaponManager weaponManager = collision.GetComponentInChildren<WeaponManager>();
            //collision.GetComponent<WeaponManager>();
        if (weaponManager != null) 
            weaponManager.EquipWeapon(weaponData);
        //Destroy(gameObject);
    }



}
