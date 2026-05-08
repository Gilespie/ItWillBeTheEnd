// PavilionDirector.cs
using System.Collections.Generic;
using UnityEngine;

public class PavilionDirector : MonoBehaviour
{
    [Header("Сценарий павильона")]
    public DirectorRule[] rules;

    private HashSet<TriggerSignal> _subscribedSignals = new HashSet<TriggerSignal>();

    private void OnEnable()
    {
        foreach (var rule in rules)
        {
            if (rule.signal != null && !_subscribedSignals.Contains(rule.signal))
            {
                rule.signal.OnFired += HandleSignal;
                _subscribedSignals.Add(rule.signal);
            }
        }
    }

    private void OnDisable()
    {
        foreach (var signal in _subscribedSignals)
        {
            if (signal != null)
            {
                signal.OnFired -= HandleSignal;
            }
        }
        _subscribedSignals.Clear();
    }

    private void HandleSignal(TriggerSignal firedSignal)
    {
        foreach (var rule in rules)
        {
            if (rule.signal == firedSignal && rule.receiver != null)
            {
                IDirectorActor actor = rule.receiver.GetComponent<IDirectorActor>();
                
                if (actor != null)
                {
                    actor.ReceiveCommand(rule.context);
                }
                else
                {
                    Debug.LogWarning($"[PavilionDirector] Объект {rule.receiver.name} не имеет интерфейса IDirectorActor!");
                }
            }
        }
    }
}