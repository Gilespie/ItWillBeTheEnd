using System.Collections;
using UnityEngine;

public class CameraPointFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _lookTarget;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _lerpRate = 3f;
    [SerializeField] private float _xLimit = 0f;
    [SerializeField] private float _yLimit = 0f;
    [SerializeField] private float _zPosMax = -15f;
    [SerializeField] private float _zPosMin = -11f;
    private Vector3 _currentPosition;
    private Vector3 _desiredPosition;

    [SerializeField] private float _transitionTime = 1f;
    [SerializeField] private float _delayTime = 8f;

    private Vector3 _shakeOffset;
    private Vector3 _defaultOffset;

    private Transform _fixedPoint = null;

    private void Awake()
    {
        GameManager.Instance.CameraPoint = this;
    }

    private void Start()
    {
        _target = GameManager.Instance.PointFollower.transform;
        _lerpRate = 1000;
        _currentPosition = _target.position + _offset;
        transform.position = _currentPosition;

        _lookTarget = _target;
        _lerpRate = 18;

        if (GameManager.Instance.ActualCheckpoint != Vector3.zero)
        {
            transform.position = GameManager.Instance.ActualCheckpoint;
        }
        else
        {
            transform.position = _target.position;
        }
    }

    private void FixedUpdate()
    {
        
        if (_fixedPoint != null)
        {
            
            transform.position = Vector3.MoveTowards(transform.position,
                _fixedPoint.position,
                _lerpRate * Time.fixedDeltaTime);
        }
        else
        {
            // если вне зоны Ч следим за игроком
            _currentPosition = _target.position + _offset + _shakeOffset;
            _currentPosition.z = _zPosMax;

            transform.position = Vector3.MoveTowards(
                transform.position,
                _currentPosition,
                _lerpRate * Time.fixedDeltaTime
            );
        }

        if (_lookTarget != null)
        {
            Vector3 direction = _lookTarget.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3f);
        }
    }

    public void SetPointView(Transform point)
    {
        _fixedPoint = point;
    }

    public void SetTargetView(Transform target)
    {
        _lookTarget = target;
    }

    public void ResetToFollow()
    {
        _fixedPoint = null;
        _lookTarget = _target;
    }

    public void SetShakeOffset(Vector3 offset)
    {
        _shakeOffset = offset;
    }

    public void ChangeOffsetAndReturn(Vector3 newOffset)
    {
        StartCoroutine(SmoothOffsetChange(newOffset));
    }

    private IEnumerator SmoothOffsetChange(Vector3 newOffset)
    {
        _defaultOffset = _offset;

        Vector3 startOffset = _offset;

        float elapsed = 0f;

        while (elapsed < _transitionTime)
        {
            _offset = Vector3.Lerp(startOffset, newOffset, elapsed / _transitionTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _offset = newOffset;

        yield return new WaitForSeconds(_delayTime);

        elapsed = 0f;

        while (elapsed < _transitionTime)
        {
            _offset = Vector3.Lerp(newOffset, _defaultOffset, elapsed / _transitionTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _offset = _defaultOffset;

        yield return null;
    }
}