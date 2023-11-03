using System.Collections;
using UnityEngine;

public class PickupFlashlight : Interactable, IInteractable
{
    public GameObject flashlightOnGround;
    public GameObject lightInRoom2;
    Flashlight playerFlashlight;
    DialogueAPI dialogue;
    
    void Start()
    {
        dialogue = GetComponent<DialogueAPI>();
        playerFlashlight = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<Flashlight>();
    }

    public void Interact()
    {
        StartCoroutine(Dialogue());
    }

    IEnumerator Dialogue()
    {
        dialogue.PlayDialogue();
        while (!dialogue.DialogueOver())
        {
            yield return null;
        }
        GameObject.Destroy(flashlightOnGround);
        GameObject.Destroy(lightInRoom2);
        playerFlashlight.hasFlashlight = true;
    }
}
