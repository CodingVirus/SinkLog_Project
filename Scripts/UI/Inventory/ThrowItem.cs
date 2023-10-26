using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThrowItem : MonoBehaviour, IDropHandler
{
    [SerializeField] GameObject noticeUI;
    private GameObject throwItem;

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        var dropItem = eventData.pointerDrag;
        throwItem = dropItem;
        if (dropItem.layer == LayerMask.NameToLayer("Item"))
        {
            noticeUI.SetActive(true);
        }
    }

    public void ItemThrow()
    {
        if (throwItem.transform.parent.parent.name == "Slot_0")
        {
            WeaponManager weaponManager = Inventory.Instance.player.GetComponentInChildren<WeaponManager>();
            weaponManager.ReleaseWeapon();
        }
        throwItem.transform.parent.GetComponent<Slot>().isEmpty = true;
        Destroy(throwItem);
        noticeUI.SetActive(false);
    }
}
