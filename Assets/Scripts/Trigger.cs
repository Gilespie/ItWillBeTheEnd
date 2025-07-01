using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] protected bool _isOnce = false;
    [SerializeField] protected UnityEvent _actions;
    protected Collider _collider;

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (_isOnce)
        {
            if (other.GetComponent<Player>())
            {
                _actions?.Invoke();
                _collider.enabled = false;
            }
        }
        else
        {
            if (other.GetComponent<Player>())
            {
                _actions?.Invoke();
            }
        }
    }
}