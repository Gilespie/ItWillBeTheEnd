using UnityEngine;

public class InputController// : MonoBehaviour
{
    [SerializeField] private TypeOfInput _typeInput = TypeOfInput.Press;

    /*public delegate void ControlDelegate();
    public ControlDelegate Control;*/

    [Header("Buttons")]
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private KeyCode _crouchKey;
    [SerializeField] private KeyCode _interactKey;
    [SerializeField] private KeyCode _timelapseKey;
    [SerializeField] private KeyCode _ragdollKey;

    [Header("References")]
    [SerializeField] private AnimationController _animController;
    [SerializeField] private Movement _movement;
    Vector3 _direction;

    public InputController(AnimationController animcontroller, Movement movement)
    {
        _movement = movement;
        _animController = animcontroller;
    }

    public void ArtificialUpdate()
    {
        _direction.x = Input.GetAxisRaw("Horizontal");
        _direction.z = Input.GetAxisRaw("Vertical");

        switch (_typeInput)
        {
            case TypeOfInput.Press:

                if (Input.GetKeyDown(_jumpKey))
                   _movement.JumpPlayer();

                if (Input.GetKeyDown(_crouchKey))
                    _movement.Crouch();
           

                break;

            case TypeOfInput.Holding:

                break;

            case TypeOfInput.Mix:

                break;
        }
    }

    public void ArtificialFixedUpdate()
    {
        if(_direction.sqrMagnitude != 0.0f)
        {
            _movement.MovePlayer(_direction);
        }
    }
}