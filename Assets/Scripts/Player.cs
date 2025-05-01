using UnityEngine;

public class Player : Destructable
{
    [Header("Inputs")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _crouchKey = KeyCode.C;
    [SerializeField] private KeyCode _sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode _shakeKey = KeyCode.Z;
    [SerializeField] private KeyCode _pushingKey = KeyCode.E;
    [SerializeField] private KeyCode _ragdollKey = KeyCode.R;
    [SerializeField] private KeyCode _pressingKey = KeyCode.F;

    [Header("Animator")]
    [SerializeField] private string _moveBoolName = "isMoving";
    [SerializeField] private string _airBoolName = "isOnAir";
    [SerializeField] private string _jumpTriggerName = "onJump";
    [SerializeField] private string _crouchBoolName = "isCrouch";
    [SerializeField] private string _pushingBoolName = "isPushing";
    [SerializeField] private string _pressTriggerName = "onPressed";
    [SerializeField] private string _xAxisName = "xAxis";
    [SerializeField] private string _zAxisName = "zAxis";

    [Header("Physics")]
    [SerializeField] private float gravity = -20f;
    [SerializeField] private Raycasting _raycast;
    [SerializeField] private Ragdoll _ragdoll;
    private bool _isRagdoll = false;

    [Header("Z Limits")]
    [SerializeField] private float _zPosMin = -5f;
    [SerializeField] private float _zPosMax = 5f;

    [Header("Parameters")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 15f;
    [SerializeField] private float _crouchSpeed = 5f;
    [SerializeField] private float _pushingSpeed = 3f;
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private CameraFollower _follower;
    [SerializeField] private bool _isGrounded = false;
    private bool _isSprinting = false;
    private bool _isCrouch = false;
    private bool _isPushing = false;
    public bool IsPushing => _isPushing;

    private bool _isInteractable = false;

    [Header("Falling")]
    [SerializeField] private float _fallDamageMultiplier = 100f;
    [SerializeField] private float _fallDamageThreshold = -10f;

    public DeathScreenManager deathScreenManager;

    private bool _wasGround = false;
    private float _maxFallSpeed = 0f;
    private float _fallDamage = 0f;

    private float _currentSpeed = 0f;
    private Vector3 _direction;
    private Rigidbody _rb;
    private CapsuleCollider _col;
    private Animator _animator;

    protected override void Awake()
    {
        Physics.gravity = new(0, gravity, 0);
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _animator = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        CheckZPosition();

        _direction.x = Input.GetAxis("Horizontal");
        _animator.SetFloat(_xAxisName, _direction.x);
        _direction.z = Input.GetAxis("Vertical");
        _animator.SetFloat(_zAxisName, _direction.z);

        _isGrounded = _raycast.IsGrounded();
        _isInteractable = _raycast.IsInteract();

        _animator.SetBool(_airBoolName, !_isGrounded);
        _animator.SetBool(_moveBoolName, _direction.sqrMagnitude != 0f);

        ChangeSpeed(_moveSpeed);

        if (Input.GetKeyDown(_ragdollKey))
        {
           _isRagdoll = !_isRagdoll;
        }

        if (Input.GetKeyDown(_jumpKey) && _isGrounded && !_isCrouch)
        {
            _animator.SetTrigger(_jumpTriggerName);
            JumpPlayer();
        }

        if (Input.GetKeyDown(_pressingKey) && _isGrounded && _isInteractable)
        {
            _animator.SetTrigger(_pressTriggerName);
            _raycast.Interact();
            Pressing();
        }

        if (Input.GetKey(_sprintKey) && _isGrounded)
        {
            ChangeSpeed(_sprintSpeed);
        }

        if (_isRagdoll)
        {
            _ragdoll.ActivateRagdoll();
        }
        else
        { 
            _ragdoll.DisableRagdoll();
        }

        if (Input.GetKeyDown(_crouchKey) && _isGrounded)
        {
            _isCrouch = !_isCrouch;
        }

        if (Input.GetKeyDown(_pushingKey) && _isGrounded)
        {
            _isPushing = !_isPushing;
        }

        if (_isPushing)
        {
            Pushing();
        }
        else
        {
            StopPushing();
        }

        if (_isCrouch)
        {
            Crouch();
        }
        else
        {
            Uncrouch();
        }

        CalculateFallDamage();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_direction.sqrMagnitude != 0.0f && _isAlive)
        {
            MovePlayer(_direction);
        }
    }

    public float ChangeSpeed(float speed)
    {
        _currentSpeed = speed;

        return _currentSpeed;
    }

    private void CalculateFallDamage()
    {
        if(!_isGrounded)
        {
            if(_rb.velocity.y < _maxFallSpeed)
            {
                _maxFallSpeed = _rb.velocity.y;
            }
        }

        if(_isGrounded && !_wasGround)
        {
            if(_maxFallSpeed < _fallDamageThreshold)
            {
                _fallDamage = Mathf.Abs(_maxFallSpeed + _fallDamageThreshold) * _fallDamageMultiplier;
                TakeDamage(_fallDamage);
            }

            _maxFallSpeed = 0f;
        }

        _wasGround = _isGrounded;
    }

    private void MovePlayer(Vector3 dir)
    {
        Vector3 moveDir = (transform.right * dir.x + transform.forward * dir.z).normalized;

        _rb.MovePosition(transform.position + moveDir * _currentSpeed * Time.fixedDeltaTime);

       /* if (moveDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = targetRot;
        }*/
    }

    public float GetSpeed()
    {
        return _currentSpeed;
    }

    private void JumpPlayer()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void Crouch()
    {
        ChangeSpeed(_crouchSpeed);
        _currentSpeed = _crouchSpeed;
        _col.height = 1;
        _col.center = new Vector3(_col.center.x, 0.5f, _col.center.z);
        _animator.SetBool(_crouchBoolName, true);
    }

    private void Uncrouch()
    {
        _animator.SetBool(_crouchBoolName, false);
        _col.height = 2;
        _col.center = new Vector3(_col.center.x, 1f, _col.center.z);
    }

    public void Pushing()
    {
        ChangeSpeed(_pushingSpeed);
        _animator.SetBool(_pushingBoolName, _isPushing);
    }

    public void StopPushing()
    {
        _animator.SetBool(_pushingBoolName, _isPushing);
    }    

    public void Pressing()
    {
        _raycast.Interact();
    }

    private void CheckZPosition()
    {
        if (transform.position.z >= _zPosMax)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _zPosMax);
        }
        if (transform.position.z <= _zPosMin)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _zPosMin);
        }
    }

    protected override void DeactivatePlayer()
    {
        base.DeactivatePlayer();
        _rb.constraints = RigidbodyConstraints.None;
        _ragdoll.ActivateRagdoll();
    }

    // public void ActivateRagdollOnFall()
    // {
    //     _isRagdoll = true;
    // }

    public void Die()
    {
        deathScreenManager.ShowDeathScreen();
    }
}