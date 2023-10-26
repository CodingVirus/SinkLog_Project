using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/*using static UnityEditor.Progress;*/

public class Item : MonoBehaviour, IDropHandler
{
    [SerializeField] private ItemDataSO weaponData;
    public ItemDataSO.ItemCategory itemCategory { get { return weaponData.category; } }
    public ItemDataSO GetWeaponData { get { return weaponData; } }
    public bool isEquip = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Inventory.Instance.AddItem(gameObject, itemCategory);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Inventory.Instance.AddItem(gameObject, itemCategory);
        }
    }

    IEnumerator ToolTipCourutine()
    {
        yield return new WaitForSeconds(1.5f);
        Inventory.Instance.ToolTipUpdate(weaponData);
        Inventory.Instance.ToolTipOn();
    }

    public void ToolTipOn()
    {  
        StartCoroutine(ToolTipCourutine());
    }
    public void ToolTipOff()
    {
        StopAllCoroutines();
        Inventory.Instance.ToolTipOff();
    }

    public void UseItem()
    {
        if (Input.GetKey(KeyCode.LeftControl)) 
        {
            switch (itemCategory)
            {
                case ItemDataSO.ItemCategory.Weapon:
                    gameObject.transform.parent.GetComponent<Slot>().isEmpty = true;
                    Inventory.Instance.AddItem(gameObject, itemCategory);
                    break;

                case ItemDataSO.ItemCategory.Accessory:
                    gameObject.transform.parent.GetComponent<Slot>().isEmpty = true;
                    Inventory.Instance.AddItem(gameObject, itemCategory);
                    break;
            }
        }
        
    }
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        var dropItem = eventData.pointerDrag;
        var dragItme = dropItem.GetComponent<DragItem>();
        var dragParentSlot = dragItme.parentTransfrom.GetComponent<Slot>();
        var thisParentSlot = transform.parent.GetComponent<Slot>();
        WeaponManager weaponManager = Inventory.Instance.player.GetComponentInChildren<WeaponManager>();

        if (dragParentSlot.slotType == SlotType.Inven
            && thisParentSlot.slotType == SlotType.Inven)
        {
            SwapItem(dragItme);
        }
        else if ((dragParentSlot.slotType == SlotType.Equipment 
            && thisParentSlot.slotType == SlotType.Equipment)
            ||
            (dragParentSlot.slotType == SlotType.Inven
            && thisParentSlot.slotType == SlotType.Equipment)
            ||
            (dragParentSlot.slotType == SlotType.Equipment 
            && thisParentSlot.slotType == SlotType.Inven))
        {
            if (isEquip)
            {
                isEquip = false;
                var item = dragItme.transform.GetComponent<Item>();
                weaponManager.EquipWeapon(item.GetWeaponData);
                item.isEquip = true;
            }
            else if (isEquip == false && dragParentSlot.transform.parent.name == "Slot_0")
            {
                isEquip = true;
                dragItme.GetComponent<Item>().isEquip = false;
                weaponManager.EquipWeapon(weaponData);
            }

            SwapItem(dragItme);
        }
    }

    private void SwapItem(DragItem dragItem)
    {
        var parentTransform = transform.parent.transform;
        transform.SetParent(dragItem.parentTransfrom);
        dragItem.parentTransfrom.GetComponent<Slot>().isEmpty = false;
        dragItem.parentTransfrom = parentTransform;
    }
}
