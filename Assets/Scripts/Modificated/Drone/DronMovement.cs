using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class DronMovement : MonoBehaviour
{
    [Header("Cutscene")]
    [SerializeField] bool _isCutsceneDrone = false;
    [SerializeField] Transform _targetPoint;

    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float targetChangeInterval = 2f;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _maxSpeedLimit = 5f;
    [SerializeField] private float _stopDistance = 1f;
    Vector3 _currentPosTarget;

    [Header("AI")]
    [SerializeField] private DroneStates _droneStates;
    [SerializeField] private Transform _player;
    [SerializeField] private float _viewDistance = 10f;
    [SerializeField] private float _fieldOfViewAngle = 90f;
    [SerializeField] private Transform _eyePoint;
    [SerializeField] private LayerMask _visionMask;

    [Header("AI - Light")]
    [SerializeField] private Light _droneLight;
    [SerializeField] private Color _patrolColor = Color.cyan;
    [SerializeField] private Color _chaseColor = Color.red;
    [SerializeField] private Color _searchColor = Color.yellow;

    [Header("Explosion")]
    [SerializeField] float _explosionForce = 3000;
    [SerializeField] float _explosionUpForce = 1;
    [SerializeField] float _explosionRadius = 5;
    [SerializeField] float _explosionDistance = 3;
    [SerializeField] LayerMask _damageMask;
    [SerializeField] GameObject _explosiveVFX;

    [Header("SFX")]
    [SerializeField] AudioClip _detectClip;
    [SerializeField] AudioSource _detectAudioSource;
    [SerializeField] private float _minBeepInterval = 0.1f;
    [SerializeField] private float _maxBeepInterval = 2f;
    [SerializeField] private float _maxBeepDistance = 15f;

    private float _beepTimer;
    Vector3 _currentTarget;
    Vector3 _lastSeenPosition;

    bool _isExploded = false;
    DroneStates _state = DroneStates.Patrol;

    private void Start()
    {
        _currentTarget = GetRandomTarget();
        SetLightColor(_patrolColor);
    }

    private void FixedUpdate()
    {
        if (!_isCutsceneDrone)
        {
            UpdateState();
            MoveToTarget();
            CheckExplosion();
            HandleBeepSound();
        }
        else
        {
            CutsceneDron();
        }
    }

    void UpdateState()
    {
        bool sees = CanSeePlayer();

        switch (_state)
        {
            case DroneStates.Patrol:
                if (sees) EnterChase();
                break;

            case DroneStates.Chase:
                if (sees)
                {
                    _lastSeenPosition = _player.position;
                    _currentTarget = _player.position;
                }
                else
                {
                    EnterInvestigate();
                }
                break;

            case DroneStates.Investigate:
                if (sees)
                {
                    EnterChase();
                }
                else if ((transform.position - _lastSeenPosition).sqrMagnitude < _stopDistance * _stopDistance)
                {
                    EnterPatrol();
                }
                else
                {
                    _currentTarget = _lastSeenPosition;
                }
                break;
        }
    }

    void EnterChase()
    {
        _state = DroneStates.Chase;
        SetLightColor(_chaseColor);
    }

    void EnterInvestigate()
    {
        _state = DroneStates.Investigate;
        _currentTarget = _lastSeenPosition;
        SetLightColor(_searchColor);
    }

    void EnterPatrol()
    {
        _state = DroneStates.Patrol;
        _lastSeenPosition = Vector3.zero;
        _currentTarget = GetRandomTarget();
        SetLightColor(_patrolColor);
    }

    bool CanSeePlayer()
    {
        Transform eye = _eyePoint != null ? _eyePoint : transform;
        Vector3 toPlayer = _player.position - eye.position;
        float distance = toPlayer.magnitude;

        if (distance > _viewDistance) return false;

        float angle = Vector3.Angle(transform.forward, toPlayer);
        if (angle > _fieldOfViewAngle * 0.5f) return false;

        if (Physics.Raycast(eye.position, toPlayer.normalized, out RaycastHit hit, distance, _visionMask))
        {
            return hit.transform == _player || hit.transform.IsChildOf(_player);
        }

        return false;
    }

    private Vector3 GetRandomTarget()
    {
        Vector3 pos = transform.position + Random.insideUnitSphere * 5f;
        return pos;
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

        // patrol: когда дошли до случайной точки — выбираем новую
        if (_state == DroneStates.Patrol &&
            (_currentTarget - transform.position).sqrMagnitude < _stopDistance * _stopDistance)
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

        SpawnVFX();
        Destroy(gameObject);
    }

    void SetLightColor(Color color)
    {
        if (_droneLight != null)
            _droneLight.color = color;
    }

    private void SpawnVFX()
    {
        Instantiate(_explosiveVFX, transform.position, Quaternion.identity);
    }

    void CutsceneDron()
    {
        SetLightColor(_chaseColor);
        Vector3 dir = (_targetPoint.position - transform.position);
        float distanceSQRT = dir.sqrMagnitude;

        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        _rb.MovePosition(_rb.position + dir.normalized * speed * Time.fixedDeltaTime);

        if(distanceSQRT < _stopDistance * _stopDistance)
        {
            Explode();
        }
        HandleBeepSound();
    }
    
    void HandleBeepSound()
    {
        if (_state != DroneStates.Chase) return;

        float distance = Vector3.Distance(transform.position, _player.position);
        float t = Mathf.Clamp01(distance / _maxBeepDistance);
        float interval = Mathf.Lerp(_minBeepInterval, _maxBeepInterval, t);

        _beepTimer -= Time.deltaTime;

        if (_beepTimer <= 0f)
        {
            _detectAudioSource.PlayOneShot(_detectClip);
            _beepTimer = interval;
        }
    }

    private void OnDrawGizmos()
    {
        // конус обзора
        Transform eye = _eyePoint != null ? _eyePoint : transform;
        Gizmos.color = Color.cyan;

        float halfFOV = _fieldOfViewAngle * 0.5f;
        Quaternion leftRay = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRay = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Gizmos.DrawRay(eye.position, leftRay * transform.forward * _viewDistance);
        Gizmos.DrawRay(eye.position, rightRay * transform.forward * _viewDistance);
        Gizmos.DrawRay(eye.position, transform.forward * _viewDistance);

        // текущая цель
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(_currentTarget, 0.2f);

        // радиус взрыва
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}