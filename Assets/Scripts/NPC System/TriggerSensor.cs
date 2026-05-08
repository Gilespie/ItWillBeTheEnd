// TriggerSensor.cs
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerSensor : MonoBehaviour
{
    public string targetTag = "Player";
    public TriggerSignal signalToSend;

    private void OnTriggerEnter(Collider other)
    {
        if (signalToSend != null && other.CompareTag(targetTag))
        {
            signalToSend.Fire();
        }
    }
}