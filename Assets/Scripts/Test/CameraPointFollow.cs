using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraPointFollow : MonoBehaviour
{
    [Header("Camera Follow Settings")]
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _lookTarget;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _zPosMax = -15f;
    [SerializeField] private float _zPosMin = -11f;
    [SerializeField] private bool _isStaticZ = false;

    [Header("Lerping Settings")]
    [SerializeField] bool _lerp = false;
    [SerializeField] float _lerpRate = 3f;
    [SerializeField] float _moveTowardsRate = 3f;

    [Header("Camera underwater effect")]
    [SerializeField] private AudioLowPassFilter _filter;
    [SerializeField] private AudioSource _audioSource;
    private Volume _currentUnderwaterVolume;
    private WaterZone _currentWaterZone;

    private Vector3 _shakeOffset;

    private Transform _cameraFixedPoint = null;
    bool _zoomed = false;

    private void Awake()
    {
        GameManager.Instance.CameraPoint = this;
    }

    private void Start()
    {
        //_target = GameManager.Instance.Player.transform;
        transform.position = _target.position + _offset;
        _lookTarget = _target;

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
        Vector3 targetPos;

        if (_cameraFixedPoint != null)
        {
            targetPos = _cameraFixedPoint.position;
        }
        else
        {
            targetPos = _target.position + _offset + _shakeOffset;

            if(_isStaticZ) targetPos.z = _zoomed ? _zPosMin : _zPosMax;

        }

        if(_lerp)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetPos,
                Time.deltaTime * _lerpRate
            );
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                Time.deltaTime * _moveTowardsRate
            );
        }

        if (_lookTarget != null)
        {
            Vector3 direction = _lookTarget.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 5f
            );
        }

        /*if (_fixedPoint != null)
        {
            
            transform.position = Vector3.MoveTowards(transform.position,
                _fixedPoint.position,
                _lerpRate * Time.fixedDeltaTime);
            *//*transform.position = Vector3.Lerp(transform.position,
                _fixedPoint.position,
                _lerpRate * Time.fixedDeltaTime);*//*
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
            *//*transform.position = Vector3.Lerp(
                transform.position,
                _currentPosition,
                _lerpRate * Time.fixedDeltaTime
            );*//*
        }

        if (_lookTarget != null)
        {
            Vector3 direction = _lookTarget.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3f);
        }

        if(_zoomed)
        {
            _currentPosition = _target.position + _offset + _shakeOffset;
            _currentPosition.z = _zPosMin;

            transform.position = Vector3.Lerp(
                transform.position,
                _currentPosition,
                _lerpRate * Time.fixedDeltaTime
            );
        }*/
    }

    public void SetVolume(Volume volume)
    {
        _currentUnderwaterVolume = volume;
    }

    public void SetWaterZone(WaterZone zone)
    {
        _currentWaterZone = zone;
    }

    public void EnterWater()
    {
        if (_currentUnderwaterVolume != null)
            _currentUnderwaterVolume.enabled = true;

        _filter.enabled = true;
        _audioSource.enabled = true;
        Debug.Log("Entered water");
    }

    public void ExitWater()
    {
        if (_currentUnderwaterVolume != null)
            _currentUnderwaterVolume.enabled = false;

        _currentUnderwaterVolume = null;

        _filter.enabled = false;
        _audioSource.enabled = false;
    }

    public void SetPointView(Transform point)
    {
        _cameraFixedPoint = point;
    }

    public void SetTargetView(Transform target)
    {
        _lookTarget = target;
    }

    public void ResetToFollow()
    {
        _cameraFixedPoint = null;
        _lookTarget = _target;
        _zoomed = false;
    }

    public void SetZPos()
    {
        _zoomed = true;
    }

    public void SetShakeOffset(Vector3 offset)
    {
        _shakeOffset = offset;
    }
}