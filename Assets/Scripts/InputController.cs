using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private TypeOfInput _typeInput = TypeOfInput.Press;
    public delegate void ControlDelegate();
    public ControlDelegate Control;

    [Header("Buttons")]
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private KeyCode _crouchKey;
    [SerializeField] private KeyCode _interactKey;
    [SerializeField] private KeyCode _timelapseKey;
    [SerializeField] private KeyCode _ragdollKey;

    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Movement _movement;

    [SerializeField] private bool _isGrounded = false;

    private bool _isCrouch = false;
    private bool _isPushing = false;
    private bool _isOnce = false;
    private bool _canMove = false;
    public bool CanMove => _canMove;

    private bool _isInteractable = false;
    private bool _isSlope = false;
    private bool _isCeiling = false;

    private void Start()
    {
        Control = _movement.MovePlayer;
    }

    void Update()
    {
        switch (_typeInput)
        {
            case TypeOfInput.Press:
                if (Input.GetKeyDown(_jumpKey) && _isGrounded)
                    Control = _movement.JumpPlayer;

                if (Input.GetKeyDown(_crouchKey) && _isGrounded)
                    Control = _movement.Crouch;


                break;

            case TypeOfInput.Holding:

                break;

            case TypeOfInput.Mix:

                break;
        }
    }
}