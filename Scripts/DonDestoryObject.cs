using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonDestoryObject : MonoBehaviour
{
    private void Start()
    {
        var objs = FindObjectsOfType<DonDestoryObject>();
        if(objs.Length > 1)
        {
            Debug.Log(objs.Length);
            foreach(var obj in objs)
            {
                if(obj.gameObject != gameObject)
                {
                    Destroy(obj.gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
