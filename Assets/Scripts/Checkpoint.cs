using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Collider _collider;

    void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            GameManager.Instance.ActualCheckpoint = transform.position;
            EventManager.Trigger(EventType.OnCheckpoint);
            _collider.enabled = false;
        }
    }
}