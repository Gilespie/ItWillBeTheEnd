using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : Destructable
{
    [Header("Reference")]
    [SerializeField] private Transform _mesh;

    [Header("Inputs")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _crouchKey = KeyCode.C;
    [SerializeField] private KeyCode _pushingKey = KeyCode.E;
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
    [SerializeField] private string _moveStateName = "moveState";
    [SerializeField] private string _slopeBoolName = "isSliding";

    [Header("Physics")]
    [SerializeField] private float gravity = -20f;
    [SerializeField] private Raycasting _raycast;
    [SerializeField] private Ragdoll _ragdoll;
    [SerializeField] private RotationTransform _rotationTransform;
    [SerializeField] private PhysicMaterial _physicsMaterial;

    [Header("Parameters")]
    [SerializeField] private float _speedMultiplier = 1f;
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _slopeSpeed = 8f;
    [SerializeField] private float _crouchSpeed = 5f;
    [SerializeField] private float _pushingSpeed = 1f;
    [SerializeField] private float _jumpForce = 25f;
    [SerializeField] private CameraFollower _follower;
    [SerializeField] private bool _isGrounded = false;
    private bool _isCrouch = false;
    private bool _isPushing = false;
    private bool _isOnce = false;
    private bool _canMove = false;
    public bool CanMove => _canMove;

    private bool _isInteractable = false;
    private bool _isSlope = false;
    private bool _isCeiling = false;

    [Header("Falling")]
    [SerializeField] private float _fallDamageMultiplier = 100f;
    [SerializeField] private float _fallDamageThreshold = -10f;

    [Header("SFX&VFX")]
    [SerializeField] private VFXSpawner _spawner;
    [SerializeField] private AudioClip _footstep;
    [SerializeField] private AudioClip[] _voices;
    private AudioSource _audioSource;

    public DeathScreenManager deathScreenManager;
    [SerializeField] private float _secondsUntilRestart = 3f;

    private bool _wasGround = false;
    private float _maxFallSpeed = 0f;
    private float _fallDamage = 0f;

    private float _currentSpeed = 0f;
    private Vector3 _direction;

    private Quaternion _defaultRotation;
    private Rigidbody _rb;
    private CapsuleCollider _col;
    private Animator _animator;

    protected override void Awake()
    {
        Physics.gravity = new(0, gravity, 0);

        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _animator = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();

        GameManager.Instance.Player = this;
    }

    protected override void Start()
    {
        base.Start();

        _defaultRotation = _mesh.rotation;

        if(GameManager.Instance.ActualCheckpoint == Vector3.zero)
            GameManager.Instance.ActualCheckpoint = transform.position;
        else
            transform.position = GameManager.Instance.ActualCheckpoint;
    }


    protected override void Update()
    {
        base.Update();

        if (!_isSlope)
        {
            _direction.x = Input.GetAxis("Horizontal");
            _direction.z = Input.GetAxis("Vertical");
        }
        else
        {
            _direction.z = Input.GetAxis("Vertical");
        }


        _isGrounded = _raycast.IsGrounded();
        _isInteractable = _raycast.IsInteract();
        _isPushing = _raycast.IsPushing();
        _isSlope = _raycast.IsSlope();
        _isCeiling = _raycast.IsCeiling();


        _animator.SetBool(_airBoolName, !_isGrounded);
        _animator.SetBool(_moveBoolName, _direction.sqrMagnitude != 0f);

        _animator.SetFloat(_xAxisName, _direction.x);
        _animator.SetFloat(_zAxisName, _direction.z);

        if (_isSlope)
        {
            _animator.SetBool(_slopeBoolName, _isSlope);
            _rb.maxLinearVelocity = 15f;
            _col.sharedMaterial = _physicsMaterial;
            SlopeRotateMesh();
        }
        else
        {
            _animator.SetBool(_slopeBoolName, _isSlope);
            _rb.maxLinearVelocity = float.MaxValue;
            _col.sharedMaterial = null;
        }

        CalculateFallDamage();

        if (Input.GetKeyDown(_jumpKey) && _isGrounded && !_isCrouch && !_isSlope)
        {
            _animator.SetTrigger(_jumpTriggerName);
            JumpPlayer();
        }

        /*    if (Input.GetKeyDown(_pressingKey) && _isGrounded && _isInteractable) //old version eliminar en el futuro
        {
            _animator.SetTrigger(_pressTriggerName);
            Pressing();
        }
        */

        if (Input.GetKeyDown(_crouchKey) && _isGrounded && !_isCeiling)
        {
            _isCrouch = !_isCrouch;
        }

        if (Input.GetKeyDown(_pushingKey) && _isGrounded && _isInteractable)  //new version hacer asi
        {
            _animator.SetTrigger(_pressTriggerName);
            Pressing();
        }

        if (Input.GetKey(_pushingKey) && _isGrounded && _isPushing && !_isCrouch)
        {
            if (!_canMove)
            {
                Pushing();
            }
        }
        else
        {
            if (_canMove)
            {
                StopPushing();
            }
        }

        if (_isCrouch)
        {
            Crouch();
        }
        else
        {
            Uncrouch();
        }

        if (_canMove)
        {
            ChangeSpeed(_pushingSpeed);
        }
        else if (_isCrouch)
        {
            ChangeSpeed(_crouchSpeed);
        }
        else if(_isSlope)
        {
            ChangeSpeed(_slopeSpeed);
        }
        else
        {
            ChangeSpeed(_moveSpeed);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_direction.sqrMagnitude != 0.0f && _isAlive)
        {
            MovePlayer(_direction);
        }
        else if (_direction.sqrMagnitude != 0.0f && _isAlive && _isSlope)
        {
            SlopeMovement(_direction);
        }
    }

    public void ResetPlayer()
    {
        transform.position = GameManager.Instance.ActualCheckpoint;
    }

    public void SlopeRotateMesh()
    {
        Vector3 slopeNormal = _raycast.Normal;
        Vector3 forward = _rb.velocity;

        Quaternion slopeTilt = Quaternion.FromToRotation(Vector3.up, slopeNormal);
        Vector3 adjustedForward = Vector3.ProjectOnPlane(forward, slopeNormal).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(adjustedForward, slopeNormal);

        _mesh.rotation = targetRotation;
    }

    public void SlopeMovement(Vector3 dir)
    {
        Vector3 moveDir = (transform.forward * dir.z).normalized;
        _rb.MovePosition(transform.position + moveDir  * _currentSpeed * Time.fixedDeltaTime);
    }

    public float ChangeSpeed(float speed)
    {
        _currentSpeed = speed * _speedMultiplier;
        return _currentSpeed;
    }

    public void SetSpeedMultiplier(float value)
    {
        _speedMultiplier = value;
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
        _rb.MovePosition(_rb.position + moveDir * _currentSpeed * Time.fixedDeltaTime);
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
        if (_canMove) return;

        _canMove = true;
        _raycast.Interact(); 
        _raycast.Pushing();
        _rotationTransform.enabled = false;
        _animator.SetBool(_pushingBoolName, _canMove);
    }

    public void StopPushing()
    {
        _canMove = false;
        _raycast.Interact();
        _raycast.Pushing();
        _rotationTransform.enabled = true;
        _animator.SetBool(_pushingBoolName, _canMove);
    }    

    public void Pressing()
    {
        _raycast.Interact();
    }

    public void ActivateControl()
    {
        this.enabled = true;
        _rotationTransform.enabled = true;
    }

    public void DeactivateControl()
    {
        this.enabled = false;
        _rotationTransform.enabled = false;
    }

    protected override void DeactivatePlayer()
    {
        if (_isOnce) return;

        base.DeactivatePlayer();

        _rb.isKinematic = true;
        _col.enabled = false;
        _rotationTransform.enabled = false;
        _ragdoll.ActivateRagdoll();
        _ragdoll.ActivateCollision();
        PlayVoice();

        if(!_isOnce)
        {
            StartCoroutine(GameOverPanel());
            _spawner.SpawnParticle(transform);
            _isOnce = true;
        }
    }

    private IEnumerator GameOverPanel()
    {
        yield return new WaitForSeconds(_secondsUntilRestart);
        Die();

        yield return null;
    }

    public void Die()
    {
        deathScreenManager.ActivateFadeIn();
    }

    public void PlayFootStep()
    {
        _audioSource.pitch = Random.Range(0.7f, 1.3f);
        _audioSource.PlayOneShot(_footstep);
    }

    public void PlayVoice()
    {
        int index = Random.Range(0, _voices.Length);
        _audioSource.PlayOneShot(_voices[index]);
    }
}