using UnityEngine;

public class DronMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float targetChangeInterval = 2f;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _maxSpeedLimit = 5f;
    [SerializeField] private float _stopDistance = 1f;
    Vector3 _currentPosTarget;

    [Header("AI")]
    [SerializeField] private Transform _player;
    [SerializeField] private float _viewDistance = 10f;
    [SerializeField] private LayerMask _visionMask;

    [Header("Debug")]
    [SerializeField] float _explosionForce = 3000;
    [SerializeField] float _explosionUpForce = 1;
    [SerializeField] float _explosionRadius = 5;
    [SerializeField] float _explosionDistance = 3;
    [SerializeField] LayerMask _damageMask;

    Vector3 _currentTarget;
    Vector3 _lastSeenPosition;

    bool _isExploded = false;
    bool _seePlayer = false;

    private void Start()
    {
        _currentTarget = GetRandomTarget();
    }

    private void FixedUpdate()
    {
        CheckPlayer();

        if (_seePlayer)
        {
            _currentTarget = _player.position;
            _lastSeenPosition = _player.position;
        }
        else
        {
            // если потеряли игрока — идём к последней точке
            if (_lastSeenPosition != Vector3.zero)
            {
                _currentTarget = _lastSeenPosition;

                if ((transform.position - _lastSeenPosition).sqrMagnitude < _stopDistance * _stopDistance)
                {
                    _lastSeenPosition = Vector3.zero;
                    _currentTarget = GetRandomTarget();
                }
            }
        }

        MoveToTarget();

        CheckExplosion();
    }

    private Vector3 GetRandomTarget()
    {
        return new Vector3(Random.Range(-10f, 10f), Random.Range(1f ,5f), Random.Range(-10f, 10f));
    }

    void CheckPlayer()
    {
        Vector3 direction = (_player.position - transform.position);
        float distance = direction.magnitude;

        if (distance > _viewDistance)
        {
            _seePlayer = false;
            return;
        }

        direction.Normalize();

        // Raycast на видимость
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, _viewDistance, _visionMask))
        {
            if (hit.transform == _player)
            {
                _seePlayer = true;
                return;
            }
        }

        _seePlayer = false;
    }

    void CheckExplosion()
    {
        if (_isExploded) return;

        float sqrDistance = (_player.position - transform.position).sqrMagnitude;

        if (sqrDistance <= _explosionDistance * _explosionDistance)
        {
            Explode();
        }
    }

    void MoveToTarget()
    {
        Vector3 direction = (_currentTarget - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        if (_rb.linearVelocity.magnitude < _maxSpeedLimit)
        {
            _rb.AddForce(transform.forward * speed, ForceMode.Force);
        }

        float sqrDistance = (_currentTarget - transform.position).sqrMagnitude;

        if (sqrDistance < _stopDistance * _stopDistance)
        {
            _currentTarget = GetRandomTarget();
        }
    }

    void Explode()
    {
        if (_isExploded) return;
        _isExploded = true;

        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius, _damageMask);

        foreach (var hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.InstantKill();

                Rigidbody[] rbs = hit.GetComponentsInChildren<Rigidbody>();

                if (rbs != null)
                {
                    foreach (Rigidbody rb in rbs)
                    {
                        rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, _explosionUpForce);
                    }
                }
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * _viewDistance);
        Gizmos.DrawSphere(_currentTarget, 0.2f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}