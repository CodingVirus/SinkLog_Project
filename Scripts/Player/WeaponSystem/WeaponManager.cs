using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Stat
{
    Transform myObject;
    [SerializeField] private Transform weaponSlot;
    [SerializeField] public ItemDataSO equippedWeapon;
    [SerializeField] public GameObject currentWeapon;
    public Transform myCursor;
    public bool isEquip = false;
    /*public GameObject ifnoWeapon;*/

    private static WeaponManager instance;
    public static WeaponManager Instance { get => instance; }

    public void EquipWeapon(ItemDataSO weaponData)
    {
        equippedWeapon = weaponData;
        if (currentWeapon != null) Destroy(currentWeapon);

        currentWeapon = Instantiate(weaponData.ItemPrefab);
        currentWeapon.transform.SetParent(weaponSlot);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
        isEquip = true;
    }

    public void ReleaseWeapon()
    {
        isEquip = false;
        equippedWeapon = null;
        Destroy(currentWeapon);
        currentWeapon= null;
    }
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        myObject = this.transform;
    }

    void Update()
    {
        if (currentWeapon != null)
        {
            AttMinDmg = equippedWeapon.attackMinDamage;
            AttMaxDmg = equippedWeapon.attackMaxDamage;
            MHP = MHP + equippedWeapon.addHP;
            DEF = equippedWeapon.defensePoint;
            CriRate = equippedWeapon.criticalRate;
            CriDmg = equippedWeapon.criticalDamage;
            AttSpeed = equippedWeapon.attackSpeed;
            MSpeed = equippedWeapon.moveSpeed;
            Dodge = equippedWeapon.dodgeRate;
        }
    }

    private void FixedUpdate()
    {
        LookCursor();
    }

    void LookCursor()   // 마우스 따라가는 함수
    {
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - myObject.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        myObject.rotation = rotation;
    }
}
