using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMinimapObj : MonoBehaviour
{
    [SerializeField] private Sprite[] spriteType;
    private Transform myFollowTarget;

    public void Initialize(Transform followTarget, GlobalEnums.IconObjType myType)
    {
        myFollowTarget = followTarget;
        GetComponent<SpriteRenderer>().sprite = spriteType[(int)myType];
        StartCoroutine(TrackingTarget());
    }

    private IEnumerator TrackingTarget()
    {
        while(myFollowTarget != null)
        {
            transform.position = myFollowTarget.position;
            yield return null;
        }
        Destroy(gameObject);
    }

}
