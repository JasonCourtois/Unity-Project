using System.Collections;
using UnityEngine;

public class AreaTransition : MonoBehaviour
{
    public bool hasFadeEffect;
    public bool transitionIntoDialogue;
    public Vector2 newCamMinPos;
    public Vector2 newCamMaxPos;
    public Vector3 newPlayerPosition;
    public bool smoothingDisabled;
    public Vector3 targetCam;
    GameObject player;
    CameraController cam;
    Animator fade;
   

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main.GetComponent<CameraController>();
        fade = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.HasTag("Player"))
        {
            return;
        }
        StartCoroutine(StartTransition());
    }

    IEnumerator StartTransition()
    {
        PlayerAttributes attributes = player.GetComponent<PlayerAttributes>();
        attributes.ChangeState(PlayerState.transition);

        if (hasFadeEffect)
        {
            fade.SetTrigger("Start");
        }
        yield return new WaitForSeconds(1f);
        fade.ResetTrigger("Start");

        player.transform.position = newPlayerPosition;
        if (smoothingDisabled) 
        {
            cam.transform.position = targetCam;
        }
        cam.minPosition = newCamMinPos;
        cam.maxPosition = newCamMaxPos;

        if (hasFadeEffect)
        {
            fade.SetTrigger("End");
        }
        yield return new WaitForSeconds(1f);

        if (!transitionIntoDialogue)
        {
            attributes.ChangeState(PlayerState.idle);
        } else
        {
            transitionIntoDialogue = false;
        }
    }   
}
