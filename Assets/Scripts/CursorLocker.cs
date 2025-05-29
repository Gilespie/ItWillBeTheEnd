using UnityEngine;

public class CursorLocker : MonoBehaviour
{
    [Header("Cursor")]
    [SerializeField] private CursorLockMode _lockState = CursorLockMode.Locked;
    [SerializeField] private bool _isCursorVisible = false;

    private void Start()
    {
        Cursor.lockState = _lockState;
        Cursor.visible = _isCursorVisible;
    }
}