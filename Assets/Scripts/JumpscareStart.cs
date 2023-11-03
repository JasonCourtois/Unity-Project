using UnityEngine;

public class JumpscareStart : MonoBehaviour
{
    DialogueAPI jumpscare;
    bool done;

    private void Start()
    {
        done = false;
        jumpscare = GetComponent<DialogueAPI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.HasTag("Player") && !collision.isTrigger && done == false)
        {
            jumpscare.PlayDialogueWithFadeOut();
            done = true;
        }
    }
}
