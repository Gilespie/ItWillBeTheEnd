using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] private bool _isOnce = false;
    [SerializeField] private UnityEvent _actions;

    private void OnTriggerEnter(Collider other)
    {
        if (_isOnce) return;

        if(other.GetComponent<Player>())
        {
            _actions?.Invoke();
            _isOnce = true;
        }
    }
}
