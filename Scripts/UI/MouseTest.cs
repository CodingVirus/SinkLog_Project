using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseTest : MonoBehaviour, IDropHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        Debug.Log("test");
    }
    // Update is called once per frame
    void Update()
    {

    }
}
