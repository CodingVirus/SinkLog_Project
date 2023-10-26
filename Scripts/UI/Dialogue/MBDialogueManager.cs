using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class MBDialogueManager : MonoBehaviour
{
    public Text txt;
    public List<Conversation> copyConversation;

    public UnityEvent DialogueStartEvents;
    public UnityEvent DialogueEndEvents;

    public GameObject conversationUI;
    public GameObject noticeUI;
    public GameObject noticeUI2;

    public Image actorImage;

    [HideInInspector] public int count = 0;
    public int spaceCount = 0;
    public float waitSecond = 0.08f;
    
    IEnumerator TextEffect()
    {
        int i = 0;
        while (true)
        {
            if (i >= copyConversation[count].text.Length)
            {
                i = 0;
                break;
            }
                
            txt.text += copyConversation[count].text[i];
            i++;
            yield return new WaitForSeconds(waitSecond);
        }
        
        yield return true;
    }
    public void StartConversation()
    {
        DialogueStartEvents.Invoke();
        //noticeUI.SetActive(false);
        noticeUI2.SetActive(false);
        ShowTriggerUI();
        
        if (copyConversation[count].whoIsTalk == null)
        {
            txt.color = Color.red;
            DialogueStart();
        }
        else
        {
            txt.color = copyConversation[count].whoIsTalk.textColor;
            DialogueStart();
        }
    }
    private void DialogueStart()
    {
        actorImage.sprite = copyConversation[count].whoIsTalk.img;
        ClearConversation();
        StartCoroutine("TextEffect");
        //copyConversation[count].eventSystem.Invoke();
    }
    public void NextConversation()
    {
        copyConversation[count].eventSystem.Invoke();
        count++;
        StopCoroutine("TextEffect");

        if (count >= copyConversation.Count) 
        {
            count = 0;
            ExitConversation();
        }

        if (copyConversation[count].whoIsTalk == null)
        {
            txt.color = Color.red;
            DialogueStart();
        }
        else
        {
            txt.color = copyConversation[count].whoIsTalk.textColor;
            DialogueStart();
        }
    }
    private void ClearConversation()
    {
        txt.text = string.Empty;
    }
    public void ExitConversation()
    {
        StopCoroutine("TextEffect");
        ClearConversation();
        count = 0;
        spaceCount = 0;
        conversationUI.SetActive(false);
        DialogueEndEvents.Invoke();
    }
    private void ShowTriggerUI()
    {
        conversationUI.SetActive(true);
    }
}