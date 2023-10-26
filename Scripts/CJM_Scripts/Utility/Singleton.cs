using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance { get =>  instance; }

    protected virtual void Awake()
    {
        instance = this as T;
        var targetObjs = FindObjectsOfType<T>();
        if (targetObjs.Length > 1)
        {
            for(int i = 0; i < targetObjs.Length; i++) 
            {
                if (targetObjs[i] != instance)
                {
                    Destroy(targetObjs[i]);
                }
            }
        }
        DontDestroyOnLoad(instance);
    }
}
