using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ItemData")]
public class ItemDataSO : ScriptableObject
{
    public enum ItemCategory
    {
        None, Weapon, Accessory
    }
    public enum WeaponType
    {
        None, MainWeapon, SubWeapon
    }
    public enum HandType
    {
        None, Onehand, Twohand
    }
    [Header("UI")]
    public string weaponName;
    public WeaponType weaponType;
    public HandType handType;
    public ItemCategory category;
    [TextArea(3, 5)]
    public string Description;

    [Header("Abillity")]
    public int addHP;
    public float attackMinDamage;
    public float attackMaxDamage;
    public float defensePoint;
    public float criticalRate;
    public float criticalDamage;
    public float attackSpeed;
    public float moveSpeed;
    public float dodgeRate;
    public GameObject ItemPrefab;

}
