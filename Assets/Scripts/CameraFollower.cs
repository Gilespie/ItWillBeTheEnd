using System.Collections;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [Header("Bob Effect")]
    [SerializeField] private bool _isBobEnable = true;
    [SerializeField] private float _bobSpeed = 3.5f;
    [SerializeField] private float _bobIntensity = 0.05f;

    [SerializeField] private float _speedRate = 5f;
    [SerializeField] private float _zPosConstant = -10f;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;

    private Vector3 _currentPosition;
    private Vector3 _shakeOffset;
    private Vector3 _bobOffset;
    private Vector3 _defaultOffset;

    private void Start()
    {
       transform.position =  _target.position;
    }

    private void Update()
    {
        if (_isBobEnable)
        {
            BobEffect(_bobSpeed,_bobIntensity);
        }
    }

    private void FixedUpdate()
    {
        _currentPosition = _target.position + _offset + _bobOffset + _shakeOffset;

        _currentPosition.z = _zPosConstant;

        transform.position = Vector3.Lerp(transform.position, _currentPosition, _speedRate * Time.fixedDeltaTime);
        transform.LookAt(_target);
    }

    public void SetShakeOffset(Vector3 offset)
    {
        _shakeOffset = offset;
    }

    public void ChangeOffset()
    {
        StartCoroutine(SmoothOffsetChange(new(_offset.x + 16f, _offset.y, _offset.z), 1f, 8f));
    }

    private void BobEffect(float speed, float intensity)
    {
        float x = Mathf.Sin(Time.time * speed) * intensity;
        float y = Mathf.Cos(Time.time * speed) * intensity;

        _bobOffset = new Vector3(x, y, 0);
    }

    private IEnumerator SmoothOffsetChange(Vector3 newOffset, float transitionTime, float returnDelay)
    {
        _defaultOffset = _offset;

        Vector3 startOffset = _offset;

        float elapsed = 0f;

        while (elapsed < transitionTime)
        {
            _offset = Vector3.Lerp(startOffset, newOffset, elapsed / transitionTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _offset = newOffset;

        yield return new WaitForSeconds(returnDelay);

        elapsed = 0f;

        while (elapsed < transitionTime)
        {
            _offset = Vector3.Lerp(newOffset, _defaultOffset, elapsed / transitionTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _offset = _defaultOffset;
    }
}