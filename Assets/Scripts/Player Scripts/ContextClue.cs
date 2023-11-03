using UnityEngine;

public class ContextClue : MonoBehaviour
{
    SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void ToggleContextClue()
    {
        sprite.enabled = !sprite.enabled;
    }
}