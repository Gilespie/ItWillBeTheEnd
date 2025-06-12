using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public event Action OnCheckpoint;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            GameManager.Instance.ActualCheckpoint = transform.position;
            OnCheckpoint?.Invoke();
            _collider.enabled = false;
        }
    }
}