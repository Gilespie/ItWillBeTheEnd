using System.Collections;
using UnityEngine;


public class Character : MonoBehaviour, IDamageable
{
    [SerializeField] bool _isAlive = true;
    public bool IsAlive => _isAlive;

    [SerializeField] CharacterAnimationController _animationController;
    [SerializeField] CharacterInputController _inputController;
    [SerializeField] GroundRaycast _groundRaycast;
    [SerializeField] SlopeRaycast _slopeRaycast;
    [SerializeField] InteractRaycast _interactRaycast;
    [SerializeField] PushingRaycast _pushingRaycast;
    [SerializeField] Rigidbody _rb;
    [SerializeField] MovementAdvance[] _movements;//0 - walk, 1 - sprint, 2 - crouch, 3 - swim, 4 - slope, 5 - push
    [SerializeField] CharacterRotator _characterRotator;
    [SerializeField] Ragdoll _ragdoll;
    [SerializeField] Collider _col;
    [SerializeField] CharacterColliderResizer _characterColliderResizer;
    [SerializeField] FallDamage _fallDamage;
    [SerializeField] InputDisabler _inputDisabler;
    MovementAdvance _currentMovement;
    bool _isJumped = false;
    bool _isCrouching = false;
    bool _isSliding = false;
    bool _isSprinting = false;
    bool _isSwimming = false;
    bool _isPushing = false;
    bool _isGround = false;
    bool _isPressingNow = false;
    bool _isGrab = false;

    bool _isPushingNow = false;
    PushableBox _currentBox;
    public PushableBox CurrentBox => _currentBox;
    Transform _currentPushPoint;


    void Awake()
    {
        _characterColliderResizer.InitDefault();

        EventManager.Subscribe(EventType.OnFalled, HandleFallDeath);
    }

    void Start()
    {
        ChangeMovement(_movements[0]);
    }

    void Update()
    {
        if(!_isAlive) return;

        _fallDamage.Tick(_isGround, _isSwimming, _isSliding, _rb.linearVelocity.y);

        _isGround = _groundRaycast.IsRaycasting(-Vector3.up);
        _isSliding = _slopeRaycast.IsRaycasting(-Vector3.up);
        _isGrab = _pushingRaycast.IsRaycasting(_characterRotator.Mesh.forward);

        if (_inputController.IsInteracting && _interactRaycast.IsRaycasting(_characterRotator.Mesh.forward))
        {
            Pressing();
        }

        if (_inputController.IsJumping && _isGround) _isJumped = true;

        _isCrouching = _inputController.IsCrouching;
        _isSprinting = _inputController.IsSprinting;

        _animationController.SetFloat(AnimParams.Speed, _currentMovement.CurrentSpeed);

        //if (_inputController.IsPushing && _isGrab) _pushingRaycast.InteractPress();

        TryStartPush();

        if (_isPushingNow && !_inputController.IsPushing)
        {
            StopPush();
        }
    }

    void FixedUpdate()
    {
        if (_isPushingNow)
        {
            ChangeMovement(_movements[5]);
        }
        else if (_isSliding)
        {
            ChangeMovement(_movements[4]);
        }
        else if (_isCrouching)
        {
            ChangeMovement(_movements[2]);
        }
        else if (_isSprinting)
        {
            ChangeMovement(_movements[1]);
        }
        else
        {
            ChangeMovement(_movements[0]);
        }

        TryJump();
        TryCrouching();
        SlideCharacter();
        UpdateCollider();

        _currentMovement.Advance(_inputController.Direction);

        if(!_isPushingNow) _characterRotator.Rotate(_inputController.Direction, _rb.linearVelocity);
    }

    void OnDestroy()
    {
        EventManager.Subscribe(EventType.OnFalled, HandleFallDeath);
    }


    void UpdateCollider()
    {
        if (!_isGround)
        {
            _characterColliderResizer.SetSize(1f, new Vector3(0, 1.5f, 0)); // воздух
        }
        else if (_isCrouching)
        {
            _characterColliderResizer.SetSize(1f, new Vector3(0, 0.5f, 0)); // crouch
        }
        else
        {
            _characterColliderResizer.SetSize(2f, new Vector3(0, 1f, 0)); // обычный
        }
    }
    void HandleFallDeath(params object[] arg)
    {
        Debug.Log("Fall death");
        InstantKill();
    }

    /*void DeactivatePlayer(params object[] parameters)
    {
        PlayVoice();

   
        if (!_isOnce)
        {
            _spawner.SpawnParticle(transform);
            StartCoroutine(GameOverPanel());
        }

    }*/

    public void InstantKill(params object[] parameters)
    {
        if(!_isAlive) return;

        _isAlive = false;
        DisableCharacter();
        ActivateDeathEffects();
        EventManager.Trigger(EventType.OnDead, _isAlive);
    }

    void DisableCharacter()
    {
        _inputDisabler.DisableControl();
        _rb.isKinematic = true;
        _col.enabled = false;
    }

    void ActivateDeathEffects()
    {
        _ragdoll.ActivateRagdoll();
        _ragdoll.ActivateCollision();

        //_spawner.SpawnParticle(transform);
        //StartCoroutine(GameOverPanel());
    }

    void ChangeMovement(MovementAdvance newMovement)
    {
        if (_currentMovement == newMovement) return;

        float prevSpeed = _currentMovement != null
        ? _currentMovement.CurrentSpeed
        : 0f;

        _currentMovement = newMovement;
        _currentMovement.Initialize(_rb);
        _currentMovement.SetSpeed(prevSpeed);

        _characterRotator.Initialize(_slopeRaycast);
    }

    void TryJump()
    {
        if (!_isJumped) return;

        _animationController.SetTrigger(AnimParams.Jump);
        _currentMovement.Jump();
        _isJumped = false;
    }

    void TryCrouching()
    {
        if (!_groundRaycast.IsRaycasting(-Vector3.up)) return;
     
        _animationController.SetBool(AnimParams.Crouch, _isCrouching);
    }

    void SlideCharacter()
    {
        _animationController.SetBool(AnimParams.Slide, _isSliding);
    }

    public void Pressing()
    {
        if (_isPressingNow) return;

        _isPressingNow = true;

        _animationController.SetTrigger(AnimParams.Press);
        _interactRaycast.InteractPress();

        StartCoroutine(ResetPress());
    }

    void TryStartPush()
    {
        if (_isPushingNow) return;

        if (_inputController.IsPushing && _isGrab)
        {
            _pushingRaycast.InteractPress(); // внутри вызовется StartPush
        }
    }


    public void StartPush(PushableBox box, Transform pushPoint)
    {
        /*Debug.Log("Start Pushing");
        _rb.position = pushPoint.position;
        transform.forward = box.transform.forward;

        _animationController.SetBool(AnimParams.Push, true);*/

        if (_isPushingNow) return;

        Debug.Log("Start Pushing");

        _isPushingNow = true;
        _currentBox = box;
        _currentPushPoint = pushPoint;
        //_characterRotator.ToggleComponent();
        _rb.position = pushPoint.position;
        _characterRotator.Mesh.forward = box.transform.right;

        _animationController.SetBool(AnimParams.Push, true);
    }

    public void StopPush()
    {
        /*Debug.Log("Stop Pushing");
        _animationController.SetBool(AnimParams.Push, false);*/

        if (!_isPushingNow) return;

        Debug.Log("Stop Pushing");
        //_characterRotator.ToggleComponent();
        _isPushingNow = false;
        _currentBox = null;
        _currentPushPoint = null;

        _animationController.SetBool(AnimParams.Push, false);
    }

    IEnumerator ResetPress()
    {
        yield return new WaitForSeconds(2f);
        _isPressingNow = false;
    }
}