using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlots : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [HideInInspector] public int InvenSlotSize = 15;
    [HideInInspector] public int EquipSlotSize = 2;
    [HideInInspector] public int AccessorySlotSize = 4;

    public SlotType slotType;

    public void SettingSlots()
    {
        switch (slotType)
        {
            case SlotType.Inven:
                InstantiateSlot(InvenSlotSize);
                break;
            case SlotType.Equipment:
                InstantiateSlot(EquipSlotSize);
                break;
            case SlotType.Accessory:
                InstantiateSlot(AccessorySlotSize);
                break;
        }
    }
    private void InstantiateSlot(int slotSize)
    {
        for (int i = 0; i < slotSize; i++)
        {
            var slot = Instantiate(slotPrefab);
            slot.name = "Slot_" + i;
            slot.transform.SetParent(transform, false);
        }
    }
}
