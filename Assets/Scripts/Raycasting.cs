using UnityEngine;

public class Raycasting : MonoBehaviour
{
    [Header("Rays")]
    [SerializeField] private Ray _groundRay;
    [SerializeField] private Ray _interactRay;
    [SerializeField] private Transform _groundOrigin;
    [SerializeField] private Transform _interactOrigin;
    private RaycastHit _groundHit;
    private RaycastHit _interactHit;

    [Header("Settings")]
    [SerializeField] private float _groundRayDistance = 0.25f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _interactRayDistance = 0.5f;
    [SerializeField] private LayerMask _interactLayer;
    [SerializeField] private float _intRadius = 5f;
    private bool _isGrounded = false;
    private bool _isInteractable = false;

    public bool IsGrounded()
    {
        _groundRay = new Ray(_groundOrigin.position, -transform.up);

        return _isGrounded = Physics.Raycast(_groundRay, _groundRayDistance, _groundLayer);
    }

    public bool IsInteract()
    {
        _interactRay = new Ray(_interactOrigin.position, transform.forward);

        return _isInteractable = Physics.Raycast(_interactRay, out _interactHit, _interactRayDistance, _interactLayer);
    }

    public void Interact()
    {
        _interactRay = new Ray(_interactOrigin.position, transform.forward);

        if (Physics.SphereCast(_interactRay, _intRadius, out _interactHit, _interactRayDistance, _interactLayer))
        {

            if(_interactHit.collider.TryGetComponent<IInteractable>(out IInteractable interact))
            {
                Debug.Log("Pressed button");
                interact.Interact();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(_groundRay.origin, _groundRay.origin + _groundRay.direction * _groundRayDistance);

        _interactRay = new Ray(_interactOrigin.position, transform.forward);
        Gizmos.color = _isInteractable ? Color.blue : Color.red;
        Gizmos.DrawLine(_interactRay.origin, _interactRay.origin + _interactRay.direction * _interactRayDistance);
    }
}