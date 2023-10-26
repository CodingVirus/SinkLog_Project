using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class Conversation
{
    public MBDialogueSetting whoIsTalk = null;

    [TextArea(1, 3)] public string text;
    [Header("Events")] public UnityEvent eventSystem;
}

[RequireComponent(typeof(MBDialogueSetting))]
public class MBDialogueControl : MonoBehaviour
{
    [SerializeField] private MBDialogueManager dialogueManager;
    public GameObject target;
    public List<Conversation> conversations;

    public bool isConversationStart = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isConversationStart) 
            {
                if (dialogueManager.spaceCount > 0)
                {
                    dialogueManager.NextConversation();
                    //dialogueManager.spaceCount++;
                }
                else
                {
                    dialogueManager.spaceCount++;
                    dialogueManager.noticeUI2.SetActive(false);
                    dialogueManager.StartConversation();
                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (isConversationStart) 
            {
                dialogueManager.ExitConversation();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            if (this.gameObject.layer == LayerMask.NameToLayer("Monologue"))
            {
                dialogueManager.copyConversation = conversations;
                dialogueManager.StartConversation();
            }
            else
            {
                isConversationStart = true;
                dialogueManager.copyConversation = conversations;
                dialogueManager.noticeUI2.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            isConversationStart = false;
            //dialogueManager.noticeUI.SetActive(false);
            dialogueManager.noticeUI2.transform.gameObject.SetActive(false);
        }
    }
}
