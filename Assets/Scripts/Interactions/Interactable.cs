using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool playerInRange;
    public bool currentlyInteracting;
    public SignalObject context;

    [HideInInspector] public PlayerAttributes player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !other.isTrigger) {
            context.Raise();
            playerInRange = true;
            player.AddTrigger(this.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player") && !other.isTrigger) {
            context.Raise();
            playerInRange = false;
            player.RemoveTrigger(this.gameObject);
        }
    }
}
