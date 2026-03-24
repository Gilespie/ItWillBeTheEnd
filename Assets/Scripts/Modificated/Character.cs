using UnityEngine;


public class Character : MonoBehaviour, IDamageable
{
    [SerializeField] bool _isAlive = true;

    [SerializeField] CharacterAnimationController _animationController;
    [SerializeField] CharacterInputController _inputController;
    [SerializeField] GroundRaycast _groundRaycast;
    [SerializeField] SlopeRaycast _slopeRaycast;
    [SerializeField] InteractRaycast _interactRaycast;
    [SerializeField] ClimbingRaycast _climbingRaycast;
    [SerializeField] Rigidbody _rb;
    [SerializeField] MovementAdvance[] _movements;//0 - walk, 1 - sprint, 2 - crouch, 3 - swim 
    [SerializeField] CharacterRotator _characterRotator;
    [SerializeField] CharacterColliderResizer _characterColliderResizer;
    MovementAdvance _currentMovement;
    bool _isJumped = false;
    bool _isCrouching = false;
    //bool _isClimbing = false;
    bool _isSprinting = false;
    bool _isGrab = false;

    void Awake()
    {
        _characterColliderResizer.InitDefault();
    }

    void Start()
    {
        ChangeMovement(_movements[0]);
    }

    void Update()
    {
        _inputController.ArtificialUpdate();

        _groundRaycast.IsRaycasting(-Vector3.up);
        _interactRaycast.IsRaycasting(transform.forward);
        _slopeRaycast.IsRaycasting(-Vector3.up);
        _climbingRaycast.IsRaycasting(transform.forward);

        if (_inputController.IsJumping) _isJumped = true;
        _isCrouching = _inputController.IsCrouching;
        _isSprinting = _inputController.IsSprinting;

        _animationController.SetBool(AnimParams.Move, _inputController.Direction.sqrMagnitude > 0.01f);
        _animationController.SetFloat(AnimParams.Speed, _currentMovement.CurrentSpeed);
    }

    void FixedUpdate()
    {
        if (_isCrouching)
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

        _currentMovement.Advance(_inputController.Direction);
        _characterRotator.Rotate(_inputController.Direction);
    }

    public void InstantKill(params object[] parameters)
    {
        _isAlive = false;
        this.enabled = false;
        EventManager.Trigger(EventType.OnDead, _isAlive);
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

        _characterRotator.Initialize(_rb);
    }

    void TryJump()
    {
        if (!_isJumped) return;
        if (!_groundRaycast.IsRaycasting(-Vector3.up)) return;

        _isJumped = false;
        _currentMovement.Jump();
        _animationController.SetTrigger(AnimParams.Jump);
    }

    void TryCrouching()
    {
        if (!_groundRaycast.IsRaycasting(-Vector3.up)) return;
     
        _animationController.SetBool(AnimParams.Crouch, _isCrouching);
        _characterColliderResizer.SetSize(_isCrouching ? 1f : 2f, _isCrouching ? new Vector3(0, 0.5f, 0) : new Vector3(0, 1f, 0));
    }
}