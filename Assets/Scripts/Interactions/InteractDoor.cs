using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDoor : Interactable, IInteractable
{
    public Animator anim;
    private bool isOpen;

    void Start()
    {
        anim = this.transform.parent.GetComponent<Animator>();
    }

    public void Interact()
    {
        if (!isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        anim.SetBool("isOpen", true);
        isOpen = true;
    }

    public void CloseDoor()
    {
        anim.SetBool("isOpen", false);
        isOpen = false;
    }
}
