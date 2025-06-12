using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] private bool _isOnce = false;
    [SerializeField] private UnityEvent _actions;
    private bool _hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isOnce && _hasTriggered) return;

        if (_isOnce)
        {
            if (other.GetComponent<Player>())
            {
                _actions?.Invoke();
                _hasTriggered = true;
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