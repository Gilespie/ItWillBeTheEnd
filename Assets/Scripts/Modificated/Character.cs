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
    [SerializeField] ClimbingRaycast _climbRaycast;
    [SerializeField] Rigidbody _rb;
    [SerializeField] MovementAdvance[] _movements;//0 - walk, 1 - sprint, 2 - crouch, 3 - swim, 4 - slope, 5 - push
    [SerializeField] CharacterRotator _characterRotator;
    [SerializeField] Ragdoll _ragdoll;
    [SerializeField] Collider _col;
    [SerializeField] CharacterColliderResizer _characterColliderResizer;
    [SerializeField] FallDamage _fallDamage;
    [SerializeField] InputDisabler _inputDisabler;
    [SerializeField] Transform _headPoint;
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
    bool _isFalling = false;
    bool _isPushingNow = false;
    bool _isClimbing = false;
    bool _isClimbingNow = false;
    bool _inWaterZone = false;

    PushableBox _currentBox;
    public PushableBox CurrentBox => _currentBox;
    Transform _currentPushPoint;
    Vector3 _climbPos;
    WaterZone _currentWaterZone;


    void Awake()
    {
        _characterColliderResizer.InitDefault();

        EventManager.Subscribe(EventType.OnFalled, HandleFallDeath);
        EventManager.Subscribe(EventType.OnFinishOxygen, InstantKill);
    }

    void Start()
    {
        ChangeMovement(_movements[0]);
    }

    void Update()
    {
        if (!_isAlive) return;

        _isFalling = _rb.linearVelocity.y < -0.1f;

        _fallDamage.Tick(_isGround, _isSwimming, _isSliding, _rb.linearVelocity.y);

        _isGround = _groundRaycast.IsRaycasting(-Vector3.up);
        _isSliding = _slopeRaycast.IsRaycasting(-Vector3.up);
        _isGrab = _pushingRaycast.IsRaycasting(_characterRotator.Mesh.forward);

        _isClimbing = _climbRaycast.IsRaycasting(_characterRotator.Mesh.forward);

        if (_currentWaterZone != null)
        {
            if (!_isSwimming && _inWaterZone && _headPoint.position.y < _currentWaterZone.BoundY)
            {
                EnterWater();
            }

            if (_isSwimming && _headPoint.position.y >= _currentWaterZone.BoundY + 0.1f && _isGround)
            {
                ExitWater();
            }
        }

        if (_inputController.IsJumping && _isClimbing && !_isClimbingNow)
        {
            StartClimb();
        }


        if (_inputController.IsInteracting && _interactRaycast.IsRaycasting(_characterRotator.Mesh.forward))
        {
            Pressing();
        }

        if (_inputController.IsJumping && _isGround) _isJumped = true;

        _isCrouching = _inputController.IsCrouching;
        _isSprinting = _inputController.IsSprinting;

        
        _animationController.SetFloat(AnimParams.Speed, _currentMovement.CurrentSpeed);

        if (_inputController.Direction.sqrMagnitude > 0.1f * 0.1f) _animationController.SetBool(AnimParams.Move, true);
        else _animationController.SetBool(AnimParams.Move, false);

        TryStartPush();

        if (_isPushingNow && !_inputController.IsPushing)
        {
            StopPush();
        }
    }

    void FixedUpdate()
    {
        if (_isSwimming)
        {
            ChangeMovement(_movements[3]);
        }
        else if (_isPushingNow)
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

        Vector3 dir = _inputController.Direction;

        if (_isSwimming)
        {
            dir.y = _inputController.VerticalSwim;

            if (_headPoint.position.y >= _currentWaterZone.BoundY && dir.y > 0f)
            {
                dir.y = 0f;
                Debug.Log("Head is above water, vertical movement disabled.");
            }
        }

        _currentMovement.Advance(dir);

        if (!_isPushingNow)
        {
            if (_isSwimming)
            {
                Vector3 swimDir = _inputController.Direction;
                swimDir.y = _inputController.VerticalSwim;

                _characterRotator.RotateSwimming(swimDir);
            }
            else
            {
                _characterRotator.Rotate(_inputController.Direction, _rb.linearVelocity);
            }
        }
    }

    void OnDestroy()
    {
        EventManager.Unsubscribe(EventType.OnFalled, HandleFallDeath);
        EventManager.Unsubscribe(EventType.OnFinishOxygen, InstantKill);
    }


    void UpdateCollider()
    {
        /*if (!_isGround && _isFalling)
        {
            _characterColliderResizer.SetSize(1f, new Vector3(0, 1.5f, 0)); // воздух
        }*/
        if (_isCrouching)
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
        if (!_isAlive) return;

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

    public void SetWaterZone(WaterZone waterZone)
    {
        _currentWaterZone = waterZone;
        _inWaterZone = waterZone != null;
    }

    public void EnterWater()
    {
        _isSwimming = true;
        _rb.useGravity = false;
        _animationController.SetTrigger(AnimParams.StartSwimm);

        ChangeMovement(_movements[3]);

        /*foreach (var particle in _bubleParticles)
        {
            particle.Play();
        }*/
    }

    public void ExitWater()
    {
        _isSwimming = false;
        _rb.useGravity = true;
        _animationController.SetTrigger(AnimParams.StopSwimm);

        ChangeMovement(_movements[0]);

        /*foreach (var particle in _bubleParticles)
        {
            particle.Stop();
        }*/
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

    public void StartClimb()
    {
        _isClimbingNow = true;
        _climbPos = _climbRaycast.LedgePoint;
        _animationController.SetTrigger(AnimParams.Climb);
    }

    public void ResetClimbing()
    {
        _isClimbingNow = false;
    }

    public void DeactivateRBKinematic()
    {
        _rb.isKinematic = false;
    }

    public void ActivateRBKinematic()
    {
        _rb.isKinematic = true;
        _rb.linearVelocity = Vector3.zero;
    }

    public void TeleportParent()
    {
        _rb.position = _climbPos;
    }
}