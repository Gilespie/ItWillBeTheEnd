using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] bool _isAutopilot = false;
    [SerializeField] float _distanceOffset = 3f;
    [SerializeField] TrainTrackBuilder _trainTrackBuilder;
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _acceleration = 5f;
    [SerializeField] float _brakeForce = 5f;
    [SerializeField] float _maxLinearSpeed = 10f;
    [SerializeField] float _drag = 2f;
    [SerializeField] float _speedRot = 2f;
    float _currentSpeed = 0f;

    private void Update()
    {
        if (_isAutopilot)
        {
            CheckWaypoint();
        }
        else
        {
            HandleInput();
        }
    }

    void FixedUpdate()
    {
        if (_isAutopilot)
        {
            ApplyDrag();
            MoveTrainAutopilot();
        }
        else
        {
            ApplyDrag();
            MoveTrain();
        }
    }

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            IncrementSpeed();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            DecrementSpeed();
        }
    }

    public void IncrementSpeed()
    {
        _currentSpeed += _acceleration * Time.deltaTime;

        _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _maxLinearSpeed);
    }

    public void DecrementSpeed()
    {
        _currentSpeed -= _brakeForce * Time.deltaTime;
    }

    void ApplyDrag()
    {
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            _currentSpeed -= _drag * Time.deltaTime;
        }

        _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, _maxLinearSpeed);
    }

    void MoveTrain()
    {
        Vector3 direction = transform.forward;
        _rb.linearVelocity = direction * _currentSpeed;
    }

    void MoveTrainAutopilot()
    {
        Vector3 targetPos = _trainTrackBuilder.GetCurrentWaypoint().position;
        Vector3 direction = (targetPos - transform.position).normalized;
        IncrementSpeed();
        _rb.linearVelocity = direction * _currentSpeed;

        Quaternion targetRot = Quaternion.LookRotation(direction);
        _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRot, _speedRot * Time.deltaTime));
    }

    void CheckWaypoint()
    {
        float distanceSqrt = (_trainTrackBuilder.GetCurrentWaypoint().position - transform.position).sqrMagnitude;

        if(distanceSqrt < _distanceOffset * _distanceOffset)
        {
            _trainTrackBuilder.NextWaypoint();
        }
    }

    public void SetAutopilot()
    {
        _isAutopilot = true;
    }
}