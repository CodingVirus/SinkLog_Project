using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour
{
    public UIResultScreen UIResultScreen;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UIResultScreen.StartShowResult(true, 112842);
        }
    }
}
