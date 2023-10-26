using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLookCursor : MonoBehaviour
{
    Transform myObject;
    public Transform setPoint;
    WeaponManager wm;

    void Start()
    {

    }

    void Update()
    {
        /*Debug.DrawRay(setPoint.position, wm.myCursor.position - setPoint.position, new Color(0, 1, 0));*//*
        if (gameObject.GetComponentInParent<WeaponManager>())
        {
            transform.rotation = new Quaternion(0, 180, -90, 0);
        }*/
    }

    private void FixedUpdate()
    {

    }
}
