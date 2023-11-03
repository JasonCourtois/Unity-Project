using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{

    private Slider chargeDisplay;
    private Light2D flashlight;
    private Animator anim;
    private Animator playerAnim;
    private PlayerAttributes playerAttributes;
    [HideInInspector] public bool isBatteryInfinite;
    public bool hasFlashlight;
    public int currentBatteryCharge;

    void Start()
    {
        chargeDisplay = GameObject.FindGameObjectWithTag("FlashlightCharge").GetComponent<Slider>();
        flashlight = GetComponent<Light2D>();
        anim = GetComponent<Animator>();
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        playerAttributes = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
        isBatteryInfinite = true;
        hasFlashlight = false;
        currentBatteryCharge = 100;
        if (flashlight.enabled && !isBatteryInfinite)
        {
            InvokeRepeating("DrainBattery", 0, 5f);
        }

    }   

    void Update()
    {
        UpdateBatteryUI();
        if (!HasCharge())
        {
            flashlight.enabled = false;
            playerAnim.SetBool("Flashlight", false);
        }
    }

    public void ToggleFlashLight()
    {
        if (!flashlight.enabled && HasCharge() && hasFlashlight)
        {
            flashlight.enabled = true;
            playerAnim.SetBool("Flashlight", true);
            if (!isBatteryInfinite)
            {
                InvokeRepeating("DrainBattery", 0, 5f);
            }
        }
        else
        {
            flashlight.enabled = false;
            playerAnim.SetBool("Flashlight", false);
            CancelInvoke("DrainBattery");
        }
    }

    private void ResetFlashlightBool()
    {
        foreach (AnimatorControllerParameter parameter in anim.parameters)
        {
            anim.SetBool(parameter.name, false);
        }
    }

    public void UpdateFlashlightDirection()
    {
        if (playerAttributes.currentDirection.Equals(PlayerDirection.down))
        {
            ResetFlashlightBool();
            anim.SetBool("down", true);
        }
        else if (playerAttributes.currentDirection.Equals(PlayerDirection.up))
        {
            ResetFlashlightBool();
            anim.SetBool("up", true);
        }
        else if (playerAttributes.currentDirection.Equals(PlayerDirection.right))
        {
            ResetFlashlightBool();
            anim.SetBool("right", true);
        }
        else if (playerAttributes.currentDirection.Equals(PlayerDirection.left))
        {
            ResetFlashlightBool();
            anim.SetBool("left", true);
        }        
    }
    void DrainBattery()
    { 
        currentBatteryCharge--;
    }

    void ReloadBattery()
    {
        currentBatteryCharge = 100;
    }

    void UpdateBatteryUI()
    { 
        int batteryLevelInDisplay = (currentBatteryCharge + 19) / 20;
        chargeDisplay.value = batteryLevelInDisplay;
    }

    bool HasCharge()
    {
        return currentBatteryCharge > 0;
    }

    public void DisableInfiniteBattery()
    {
        isBatteryInfinite = false;
        if (flashlight.enabled)
        {
            InvokeRepeating("DrainBattery", 0, 5f);
        }
    }

    public void EnableInfiniteBattery()
    {
        isBatteryInfinite = true;
        CancelInvoke("DrainBattery");
    }
}