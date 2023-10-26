using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreControl : MonoBehaviour
{
    [SerializeField] private GameObject slotPanel;
    [SerializeField] private GameObject noticeUI;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private List<GameObject> copyProductList;
    public void MakeSlots(int slotSize, List<GameObject> list)
    {
        for (int i = 0; i < slotSize; i++)
        {
            copyProductList = list;

            var slot = Instantiate(slotPrefab);
            slot.GetComponent<Button>().onClick.AddListener(()=> NoticeOn(slot));
            slot.transform.SetParent(slotPanel.transform, false);

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

    public void NoticeOn(GameObject slot)
    {
        //var itme = slot.transform.GetChild(0);
        yesButton.onClick.RemoveAllListeners();
        noticeUI.SetActive(true);
        yesButton.onClick.AddListener(()=> BuyProduct(slot));
    }
}
