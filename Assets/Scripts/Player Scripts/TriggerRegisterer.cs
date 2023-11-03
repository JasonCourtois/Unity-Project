using UnityEngine;

public class TriggerRegisterer : MonoBehaviour
{
    
    PlayerAttributes player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.HasTag("Player"))
        {
            return;
        }
        player.AddTrigger(this.gameObject);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.HasTag("Player"))
        {
            return;
        }
        player.RemoveTrigger(this.gameObject);
    }
}
