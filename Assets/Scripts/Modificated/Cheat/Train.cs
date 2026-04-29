using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] bool _isAutopilot = false;
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _acceleration = 5f;
    [SerializeField] float _brakeForce = 5f;
    [SerializeField] float _maxLinearSpeed = 10f;
    [SerializeField] float _drag = 2f;
    float _currentSpeed = 0f;

    void FixedUpdate()
    {
        if (_isAutopilot)
        {
            IncrementSpeed();
            ApplyDrag();
            MoveTrain();
        }
        else
        {
            ApplyDrag();
            MoveTrain();
        }
    }

    public void IncrementSpeed()
    {
        _currentSpeed += _acceleration * Time.fixedDeltaTime;

        _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _maxLinearSpeed);
    }

    public void DecrementSpeed()
    {
        _currentSpeed -= _brakeForce * Time.fixedDeltaTime;
    }

    void ApplyDrag()
    {
        _currentSpeed -= _drag * Time.fixedDeltaTime;
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, _maxLinearSpeed);
    }

    void MoveTrain()
    {
        Vector3 direction = transform.forward;
        _rb.linearVelocity = direction * _currentSpeed;
    }

    public void SetAutopilot()
    {
        _isAutopilot = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Character character))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            other.transform.SetParent(null);
        }
    }
}