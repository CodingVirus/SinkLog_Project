using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerSpawnPosition : MonoBehaviour
{
    [SerializeField] private Vector3 targetPosition;
    private void Awake()
    {
        FindObjectOfType<PlayerControl>().transform.parent.position = targetPosition;
        FindObjectOfType<PlayerControl>().transform.localPosition = Vector3.zero;
    }
}
