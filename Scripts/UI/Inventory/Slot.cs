using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/*using static UnityEditor.Progress;*/

public class Slot : MonoBehaviour, IDropHandler
{
    public SlotType slotType;
    [HideInInspector] public Image itemImg;
    public bool isEmpty = true;
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        var dropItem = eventData.pointerDrag;
        if (dropItem.layer == LayerMask.NameToLayer("Item")
            && this.isEmpty == true)
        {
            ItemDataSO.ItemCategory dragItemType = dropItem.GetComponent<Item>().itemCategory;
            WeaponManager weaponManager = Inventory.Instance.player.GetComponentInChildren<WeaponManager>();
            var dragItem = dropItem.GetComponent<DragItem>();

            switch (slotType)
            {
                case SlotType.Inven:
                    if (dragItemType == ItemDataSO.ItemCategory.Weapon)
                    {
                        if (dropItem.GetComponent<Item>().isEquip == true)
                        {
                            dropItem.GetComponent<Item>().isEquip = false;
                            weaponManager.ReleaseWeapon();
                        }
                    }
                    dragItem.parentTransfrom = transform;
                    break;

                case SlotType.Equipment:
                    if (dragItemType == ItemDataSO.ItemCategory.Weapon)
                    {
                        if (this.transform.parent.name == "Slot_0")
                        {
                            weaponManager.EquipWeapon(dropItem.GetComponent<Item>().GetWeaponData);
                            dragItem.GetComponent<Item>().isEquip = true;
                        }
                        else
                        {
                            if (dragItem.parentTransfrom.parent.name == "Slot_0")
                            {
                                weaponManager.ReleaseWeapon();
                            }
                            dragItem.GetComponent<Item>().isEquip = false;
                        }
                        dragItem.parentTransfrom = transform;
                    }
                    break;

                case SlotType.Accessory:
                    if (dragItemType == ItemDataSO.ItemCategory.Accessory)
                    {
                        dragItem.parentTransfrom = transform;
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
