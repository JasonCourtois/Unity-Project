using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
    public SignalObject signal; 
    public UnityEvent signalEvent;

    public void onSignalRaised() 
    {
        signalEvent.Invoke();
    }

    private void OnEnable()
    {
        signal.registerListener(this);
    }

    private void OnDisable() 
    {
        signal.deRegisterListener(this);
    }
}
