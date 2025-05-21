using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private TypeOgInput _typeInput = TypeOgInput.Press;

    [Header("Buttons")]
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private KeyCode _crouchKey;
    [SerializeField] private KeyCode _interactKey;
    [SerializeField] private KeyCode _timelapseKey;
    [SerializeField] private KeyCode _ragdollKey;

    [Header("References")]
    [SerializeField] private Animator _animator;

    void Update()
    {
        switch (_typeInput)
        {
            case TypeOgInput.Press:

                break;

            case TypeOgInput.Holding:

                break;

            case TypeOgInput.Mix:

                break;
        }
    }
}