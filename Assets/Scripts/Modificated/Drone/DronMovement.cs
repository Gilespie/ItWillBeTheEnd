using UnityEngine;

public class DronMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float targetChangeInterval = 2f;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _maxSpeedLimit = 5f;
    [SerializeField] private float _stopDistance = 1f;
    Vector3 _currentTarget;

    private void Start()
    {
        _currentTarget = GetRandomTarget();
    }

    private void FixedUpdate()
    {
        if (_currentTarget != null)
        {
            float sqrtDistance = (_currentTarget - transform.position).sqrMagnitude;

            Vector3 direction = (_currentTarget - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            _rb.AddForce(transform.forward * speed * Time.fixedDeltaTime, ForceMode.Force);

            if (_rb.linearVelocity.magnitude < _maxSpeedLimit)
            {
                _rb.AddForce(transform.forward * speed, ForceMode.Force);
            }

            if(sqrtDistance < _stopDistance * _stopDistance)
            {
                _currentTarget = GetRandomTarget();
            }
        }     
    }

    private Vector3 GetRandomTarget()
    {
        Vector3 randomTargetPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(1f ,5f), Random.Range(-10f, 10f));
        return randomTargetPosition;
    }
}