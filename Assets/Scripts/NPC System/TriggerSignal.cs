using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trigger Signal", menuName = "Architecture/Trigger Signal")]
public class TriggerSignal : ScriptableObject
{
    public event Action<TriggerSignal> OnFired;

    public void Fire()
    {
        OnFired?.Invoke(this);
    }
}