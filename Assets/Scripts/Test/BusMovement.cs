using UnityEngine;

public class BusMovement : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _speed = 5f;
    [SerializeField] Transform _targetPos;
    [SerializeField] float _stopDistance = 1f;
    bool _isMoving = false;
    
    void FixedUpdate()
    {
        if (!_isMoving) return;

        if (Vector3.Distance(transform.position, _targetPos.position) < _stopDistance)
        {
            StopMoving();
            return;
        }

        _rb.AddForce((_targetPos.position - transform.position).normalized * _speed, ForceMode.Acceleration);
    }

    public void StartMoving()
    {
        _isMoving = true;
    }

    public void StopMoving()
    {
        _isMoving = false;
    }
}
