using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessorySlot : MonoBehaviour
{
    public GameObject slotPrefab;
    public int slotSize = 2;
    private void Awake()
    {
        for (int i = 0; i < slotSize; i++)
        {
            var slot = Instantiate(slotPrefab);
            slot.transform.SetParent(transform, false);
        }
    }
}
