using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreData : MonoBehaviour
{
    [SerializeField] private GameObject storeUI;
    [SerializeField] protected List<GameObject> productList;

    private void Start()
    {
        var store = Instantiate(storeUI);
        store.transform.SetParent(transform, false);
        store.GetComponent<StoreControl>().MakeSlots(productList.Count, productList);
        storeUI = store;
        storeUI.SetActive(false);
    }

    public void StoreOn()
    {
        storeUI.SetActive(true);
    }
}
