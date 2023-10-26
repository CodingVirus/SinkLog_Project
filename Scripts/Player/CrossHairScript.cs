using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairScript : MonoBehaviour
{
    Vector2 mouseCursorPos;

    private static CrossHairScript instance;
    public static CrossHairScript Instance { get => instance; }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.visible = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mouseCursorPos;
    }
}
