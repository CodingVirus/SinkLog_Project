using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
/*using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;*/

public class Inventory : MonoBehaviour
{
    // ½Ì±ÛÅæÀ» »ç¿ëÇÏ¿© ¸Ê º¯°æ½Ã¿¡µµ À¯Áö
    private static Inventory instance = null;

    [SerializeField] public GameObject player;
    [SerializeField] private GameObject inventoryWindow;
    [SerializeField] private GameObject inven;
    [SerializeField] private GameObject equipment;
    [SerializeField] private GameObject accessory;
    [SerializeField] private GameObject toolTip;

    public void ToolTipUpdate(ItemDataSO weaponData)
    {
        toolTip.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Min Damage = " + weaponData.attackMinDamage;
        toolTip.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Max Damage = " + weaponData.attackMaxDamage;
        toolTip.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Defence Point = " + weaponData.defensePoint;
        toolTip.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Critical Rate = " + weaponData.criticalRate;
        toolTip.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Critical Damage = " + weaponData.criticalDamage;
        toolTip.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "Attack Speed = " + weaponData.attackSpeed;
    }
    // ÅøÆÁ
    public void ToolTipOn() { toolTip.SetActive(true);}
    public void ToolTipOff() { toolTip.SetActive(false);}

    private void Awake()
    {
        instance = this;
        /*if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
*/
        inven.GetComponent<InventorySlots>().SettingSlots();
        equipment.GetComponent<InventorySlots>().SettingSlots();
        accessory.GetComponent<InventorySlots>().SettingSlots();
    }
    public static Inventory Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && inventoryWindow.activeSelf == false)
        {
            inventoryWindow.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.I) && inventoryWindow.activeSelf == true)
        {
            inventoryWindow.SetActive(false);
        }
    }

    private void AddTypeOfItem(GameObject item, GameObject slotType, int slotSize)
    {
        item.transform.localScale = Vector3.one;
        int i = 0;
        for (i = 0; i < slotSize; i++)
        {     
            var slot = slotType.transform.GetChild(i);
            if (slot.GetComponent<Slot>().isEmpty == true)
            {
                item.transform.SetParent(slot.transform, false);
                slot.GetComponent<Slot>().isEmpty = false;
                item.GetComponent<Item>().isEquip = true;
                break;
            }    
        }
        if (i >= slotSize)
        {
            var invenSlotSize = inven.GetComponent<InventorySlots>().InvenSlotSize;
            for (i = 0; i < invenSlotSize; i++)
            {
                var slot = inven.transform.GetChild(i);
                if (slot.GetComponent<Slot>().isEmpty == true)
                {
                    item.transform.SetParent(slot.transform, false);
                    slot.GetComponent<Slot>().isEmpty = false;
                    item.GetComponent<Item>().isEquip = false;
                    break;
                }
            }
        }
    }
    private void AddEquipItem(GameObject item, GameObject slotType, int slotSize)
    {
        item.transform.localScale = Vector3.one;
        WeaponManager weaponManager = player.GetComponentInChildren<WeaponManager>();

        int i = 0;
        for (i = 0; i < slotSize; i++)
        {
            var slot = slotType.transform.GetChild(i).GetChild(0);
            if (slot.GetComponent<Slot>().isEmpty == true)
            {
                item.transform.SetParent(slot.transform, false);
                slot.GetComponent<Slot>().isEmpty = false;
                if (item.GetComponent<Item>().isEquip == false && i == 0)
                {
                    item.GetComponent<Item>().isEquip = true;
    
                    if (weaponManager != null)
                    {
                        var weaponData = item.transform.GetComponent<Item>().GetWeaponData;
                        weaponManager.EquipWeapon(weaponData);
                    }
                }

                break;
            }
        }
        if (i >= slotSize)
        {
            var invenSlotSize = inven.GetComponent<InventorySlots>().InvenSlotSize;
            for (i = 0; i < invenSlotSize; i++)
            {
                var slot = inven.transform.GetChild(i);
                if (slot.GetComponent<Slot>().isEmpty == true)
                {
                    item.transform.SetParent(slot.transform, false);
                    slot.GetComponent<Slot>().isEmpty = false;
                    item.GetComponent<Item>().isEquip = false;
                    break;
                }
            }
        }
    }
    public void AddItem(GameObject item, ItemDataSO.ItemCategory itemCategory)
    {
        var equipSlotSize = equipment.GetComponent<InventorySlots>().EquipSlotSize;
        var invenSlotSize = inven.GetComponent<InventorySlots>().InvenSlotSize;
        var accessorySlotSize = accessory.GetComponent<InventorySlots>().AccessorySlotSize;

        switch (itemCategory)
        {
            case ItemDataSO.ItemCategory.Weapon:
                AddEquipItem(item, equipment, equipSlotSize);
                break;

            case ItemDataSO.ItemCategory.Accessory:
                AddTypeOfItem(item, accessory, accessorySlotSize);
                break;

            case ItemDataSO.ItemCategory.None:
                AddTypeOfItem(item, inven, invenSlotSize); 
                break;
        }
        
        
    }
}
