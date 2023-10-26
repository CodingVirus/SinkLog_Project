using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerObjects : MonoBehaviour
{
    private void Awake()
    {
        var objs = FindObjectsOfType<DonDestoryObject>();
        if(objs.Length > 0)
        {
            foreach (var obj in objs)
            {
                Destroy(obj.gameObject);
            }
        }
    }
}
