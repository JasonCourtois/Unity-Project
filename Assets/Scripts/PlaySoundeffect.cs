using UnityEngine;

public class PlaySoundeffect : MonoBehaviour
{
    public AudioSource sound;

    void OnEnable()
    {
        sound.Play();
    }
}
