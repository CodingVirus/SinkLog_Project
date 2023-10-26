using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIWorldmapPlayerIcon : MonoBehaviour
{
    private Coroutine coroutine;
    public void BlinkPlayerLocation()
    {
        if (coroutine == null) 
        {
            coroutine = StartCoroutine(BlinkingPlayerLocation());
        }
    }
    private void OnDisable()
    {
        coroutine = null;
    }

    private IEnumerator BlinkingPlayerLocation()
    {
        while(true)
        {
            GetComponent<Image>().color = Color.white;
            yield return new WaitForSeconds(0.75f);
            GetComponent<Image>().color = Color.clear;
            yield return new WaitForSeconds(0.75f);
        }
    }
}
