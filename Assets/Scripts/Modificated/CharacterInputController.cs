using UnityEngine;

public class CharacterInputController : MonoBehaviour 
{
    [SerializeField] TypeOfInput _typeInput = TypeOfInput.Press;

    [Header("Buttons")]
    [SerializeField] KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] KeyCode _sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode _crouchKey = KeyCode.LeftControl;
    [SerializeField] KeyCode _interactKey = KeyCode.E;
    [SerializeField] KeyCode _slowTimeKey = KeyCode.T;
    [SerializeField] KeyCode _ragdollKey = KeyCode.R;

    bool _isCrouching = false;
    public bool IsCrouching => _isCrouching;

    bool _isJumping = false;
    public bool IsJumping => _isJumping;

    bool _isSprinting = false;
    public bool IsSprinting => _isSprinting;

    bool _isInteracting = false;
    public bool IsInteracting => _isInteracting;

    bool _isRagdoll = false;
    public bool IsRagdoll => _isRagdoll;

    bool _isSlowTime = false;
    public bool ISSlowTime => _isSlowTime;

    Vector3 _direction;
    public Vector3 Direction => _direction;

    public void ArtificialUpdate()
    {
        _direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"), Input.GetAxis("Vertical"));

        switch (_typeInput)
        {
            case TypeOfInput.Press:
                if (Input.GetKeyDown(_crouchKey)) _isCrouching = !_isCrouching;
                if(Input.GetKeyDown(_sprintKey)) _isSprinting = !_isSprinting;
                if(Input.GetKeyDown(_ragdollKey)) _isRagdoll = !_isRagdoll;
                if(Input.GetKeyDown(_slowTimeKey)) _isSlowTime = !_isSlowTime;

                _isJumping = Input.GetKeyDown(_jumpKey);
                _isInteracting = Input.GetKeyDown(_interactKey);
                break;

            case TypeOfInput.Holding:
                _isCrouching = Input.GetKey(_crouchKey);
                _isSprinting = Input.GetKey(_sprintKey);

                _isJumping = Input.GetKeyDown(_jumpKey);
                _isInteracting = Input.GetKey(_interactKey);
                break;
        }
    }
}