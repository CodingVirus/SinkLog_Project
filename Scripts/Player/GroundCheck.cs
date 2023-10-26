using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Transform rayCheck;
    [SerializeField] private Transform playerFeet;
    [SerializeField] private LayerMask platformLayer;
    private RaycastHit2D raycheck;
    void PlatformCheckMethod()
    {
        raycheck = Physics2D.Raycast(rayCheck.position, -Vector2.up, 100.0f, platformLayer);
        if (raycheck != false)
        {
            Vector2 temp = playerFeet.position;
            temp.y = raycheck.point.y;
            playerFeet.position = temp;
        }
    }
    void Update()
    {
        PlatformCheckMethod();
    }
}
