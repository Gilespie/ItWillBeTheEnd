using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private string _moveBoolName = "isMoving";
    [SerializeField] private string _airBoolName = "isOnAir";
    [SerializeField] private string _jumpTriggerName = "onJump";
    [SerializeField] private string _crouchBoolName = "isCrouch";
    [SerializeField] private string _pushingBoolName = "isPushing";
    [SerializeField] private string _pressTriggerName = "onPressed";
    [SerializeField] private string _xAxisName = "xAxis";
    [SerializeField] private string _zAxisName = "zAxis";
    [SerializeField] private string _moveStateName = "moveState";
    [SerializeField] private string _slopeBoolName = "isSliding";
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void ChangeState(string parameter, bool state)
    {
        _animator.SetBool(parameter, state);
    }
}