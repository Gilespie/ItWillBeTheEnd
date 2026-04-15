using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] protected bool _isOnce = false;
    [SerializeField] protected UnityEvent _actions;
    [SerializeField] protected UnityEvent _actionsStay;
    protected Collider _collider;

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (_isOnce)
        {
            if (other.GetComponent<Character>())
            {
                _actions?.Invoke();
                _collider.enabled = false;
            }
        }
        else
        {
            if (other.GetComponent<Character>())
            {
                _actions?.Invoke();
            }
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Character>())
        {
            _actionsStay?.Invoke();
        }
    }

    [ContextMenu("Activate Trigger")]
    public void Activate()
    {
        _actions?.Invoke();
    }
}