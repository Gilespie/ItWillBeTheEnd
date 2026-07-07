using UnityEngine;

public class CharacterInputController : MonoBehaviour 
{
    [Header("Buttons")]
    [SerializeField] KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] KeyCode _sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode _crouchKey = KeyCode.LeftControl;
    [SerializeField] KeyCode _interactKey = KeyCode.E;
    [SerializeField] KeyCode _pushingKey = KeyCode.F;
    [SerializeField] KeyCode _pauseKey = KeyCode.Escape;

    bool _isCrouching = false;
    public bool IsCrouching => _isCrouching;

    bool _isJumping = false;
    public bool IsJumping => _isJumping;

    bool _isSprinting = false;
    public bool IsSprinting => _isSprinting;

    bool _isInteracting = false;
    public bool IsInteracting => _isInteracting;

    bool _isPushing = false;
    public bool IsPushing => _isPushing;

    bool _isRagdoll = false;
    public bool IsRagdoll => _isRagdoll;

    bool _isSlowTime = false;
    public bool ISSlowTime => _isSlowTime;

    Vector3 _direction;
    public Vector3 Direction => _direction;

    bool _isActive = true;

    bool _isPaused = false;

    public bool IsPressedAny => Input.anyKey;

    public float VerticalSwim => Input.GetAxis("Jump") > 0 ? 1 : (Input.GetKey(KeyCode.LeftControl) ? -1 : 0);

    void Update()
    {
        if (!_isActive) return;

        _direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"), Input.GetAxis("Vertical"));

        _isCrouching = Input.GetKey(_crouchKey);
        _isSprinting = Input.GetKey(_sprintKey);

        _isJumping = Input.GetKeyDown(_jumpKey);
        _isInteracting = Input.GetKeyDown(_interactKey);
        _isPushing = Input.GetKey(_pushingKey);
        
        if(Input.GetKeyDown(_pauseKey))
        {
            _isPaused = !_isPaused;
            EventManager.Trigger(EventType.OnPaused, _isPaused);
        }
    }

    public void ToggleComponent()
    {
        _isActive = !_isActive;
        _direction = Vector3.zero;
    }
}