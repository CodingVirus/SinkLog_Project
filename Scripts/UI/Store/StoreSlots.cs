using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StoreSlots : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    private List<GameObject> copyProductList;
    public void MakeSlots(int slotSize, List<GameObject> list)
    {
        for (int i = 0; i < slotSize; i++)
        {
            copyProductList = list;

            var slot = Instantiate(slotPrefab);
            slot.GetComponent<Button>().onClick.AddListener(() => BuyProduct(slot));
            slot.transform.SetParent(transform, false);
            
            var product = Instantiate(copyProductList[i]);
            product.GetComponent<Image>().raycastTarget = false;
            product.transform.SetParent(slot.transform, false);
        }
    }
    public void BuyProduct(GameObject slot)
    {
        var item = slot.transform.GetChild(0);
        item.GetComponent<Image>().raycastTarget = true;
        Inventory.Instance.AddItem(item.gameObject, item.GetComponent<Item>().itemCategory);
        Destroy(slot);
    }
}
