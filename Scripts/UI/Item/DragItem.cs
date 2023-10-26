using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentTransfrom;
    
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<Image>().raycastTarget = false;
        parentTransfrom = transform.parent;
        //transform.SetParent(transform.root);
        transform.SetParent(Inventory.Instance.transform);
    }

    void IDragHandler.OnDrag(PointerEventData eventData) 
    {
        parentTransfrom.GetComponent<Slot>().isEmpty = true;
        transform.position = Input.mousePosition;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData) 
    {
        parentTransfrom.GetComponent<Slot>().isEmpty = false;
        transform.SetParent(parentTransfrom);
        GetComponent<Image>().raycastTarget = true;
    }
}
