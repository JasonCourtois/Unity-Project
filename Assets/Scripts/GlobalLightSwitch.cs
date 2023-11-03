using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLightSwitch : MonoBehaviour, IInteractable
{
    private Light2D globalLight;
    private Light2D playerLight;
    // Start is called before the first frame update
    void Start()
    {
        globalLight = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>();
        playerLight = GameObject.FindGameObjectWithTag("PlayerLight").GetComponent<Light2D>();
    }

    // Toggles global lightswitch
    public void Interact()
    {
        globalLight.enabled = !globalLight.enabled;
        playerLight.enabled = !playerLight.enabled;
    }
}
