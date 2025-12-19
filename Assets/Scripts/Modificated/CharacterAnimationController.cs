using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] string _moveBoolName = "isMoving";
    [SerializeField] string _airBoolName = "isOnAir";
    [SerializeField] string _crouchBoolName = "isCrouch";
    [SerializeField] string _pushingBoolName = "isPushing";
    [SerializeField] string _slopeBoolName = "isSliding";
    [SerializeField] string _jumpTriggerName = "onJump";
    [SerializeField] string _pressTriggerName = "onPressed";
    [SerializeField] string _xAxisName = "xAxis";
    [SerializeField] string _yAxisName = "yAxis";
    [SerializeField] string _zAxisName = "zAxis";
    [SerializeField] string _moveStateName = "moveState";
    [SerializeField] string _startSwimTriggerName = "onStartSwimming";
    [SerializeField] string _stopSwimTriggerName = "onStopSwimming";
    Animator _animator;

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void SetBoolMove(bool value)
    {
        _animator.SetBool(_moveBoolName, value);
    }

    public void SetBoolInAir(bool value)
    {
        _animator.SetBool(_moveBoolName, value);
    }

    public void SetBoolCrouch(bool value)
    {
        _animator.SetBool(_moveBoolName, value);
    }

    public void SetBoolPushing(bool value)
    {
        _animator.SetBool(_moveBoolName, value);
    }

    public void SetBoolSlope(bool value)
    {
        _animator.SetBool(_moveBoolName, value);
    }

    public void SetTriggerJump()
    {
        _animator.SetTrigger(_jumpTriggerName);
    }

    public void SetTirggerPress()
    {
        _animator.SetTrigger(_pressTriggerName);
    }

    public void SetTirggerStartSwim()
    {
        _animator.SetTrigger(_startSwimTriggerName);
    }

    public void SetTirggerStopSwim()
    {
        _animator.SetTrigger(_stopSwimTriggerName);
    }

    public void SetFloatXAxis(float value)
    {
        _animator.SetFloat(_xAxisName, value);
    }

    public void SetFloatYAxis(float value)
    {
        _animator.SetFloat(_yAxisName, value);
    }

    public void SetFloatZAxis(float value)
    {
        _animator.SetFloat(_zAxisName, value);
    }
}