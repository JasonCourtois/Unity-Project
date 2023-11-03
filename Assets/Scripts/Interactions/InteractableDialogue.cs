using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableDialogue : Interactable
{
    public enum dialogueState {
        defaultDialogue,
        preQuest,
        postQuest,
        finishedQuest
    }

    public dialogueState currentState;
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public DialogueList dialogue;
    public int dialogueIndex = 0;

    public bool done;

    
    
    
    public void playDialogue() {
        if (!playerInRange)
        {
            return;
        }
        Debug.Log("test");
        if (!dialogueBox.activeInHierarchy) 
        {
            Debug.Log("1");
            progressDialogue();
            openDialogueUI();
        } 
        else 
        {
            Debug.Log("2");
            progressDialogue();
        }
    }

    void openDialogueUI() 
    {
        Debug.Log("open");
        dialogueBox.SetActive(true);
        done = false;
    }

    void closeDialogueUI() 
    {
        dialogueIndex = 0;
        dialogueBox.SetActive(false);
    }

    void progressDialogue()
     {
        Debug.Log("progress");
        if (currentState == dialogueState.defaultDialogue)
        {
            if (dialogueIndex < dialogue.defaultDialogue.Count) 
            {
                dialogueText.text = dialogue.defaultDialogue[dialogueIndex++];
            }
            else 
            {
                done = true;
                closeDialogueUI();
            }
        }
        else if (currentState == dialogueState.preQuest)
        {
            if (dialogueIndex < dialogue.preAcceptQuest.Count) 
            {
                dialogueText.text = dialogue.preAcceptQuest[dialogueIndex++];
            }
            else 
            {
                done = true;
                closeDialogueUI();
            }
        }
        else if (currentState == dialogueState.postQuest)
        {
            if (dialogueIndex < dialogue.postAcceptQuest.Count) 
            {
                dialogueText.text = dialogue.postAcceptQuest[dialogueIndex++];
            }
            else 
            {
                done = true;
                closeDialogueUI();
            }
        }
        else if (currentState == dialogueState.finishedQuest)
        {
            if (dialogueIndex < dialogue.finishedQuest.Count) 
            {
                dialogueText.text = dialogue.finishedQuest[dialogueIndex++];
            }
            else 
            {
                done = true;
                closeDialogueUI();
                currentState = dialogueState.defaultDialogue;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Player")) 
        {
            context.Raise();
            playerInRange = false;
            closeDialogueUI();
        }
    }
}
