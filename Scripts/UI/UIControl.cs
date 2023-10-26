using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C) && transform.GetChild(0).gameObject.activeSelf == false)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true); 
        }
        else if (Input.GetKeyDown(KeyCode.C) && transform.GetChild(0).gameObject.activeSelf == true)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
