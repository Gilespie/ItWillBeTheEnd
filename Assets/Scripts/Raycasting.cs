using UnityEngine;

public class Raycasting : MonoBehaviour
{
    [Header("Rays")]
    [SerializeField] private Ray _groundRay;
    [SerializeField] private Ray _interactRay;
    [SerializeField] private Ray _pushingRay;
    [SerializeField] private Transform _groundOrigin;
    [SerializeField] private Transform _interactOrigin;
    private RaycastHit _groundHit;
    private RaycastHit _interactHit;
    private RaycastHit _pushingHit;

    [Header("Settings")]
    [SerializeField] private float _groundRayDistance = 0.45f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _interactRayDistance = 0.5f;
    [SerializeField] private LayerMask _interactLayer; 
    [SerializeField] private float _pushingRayDistance = 0.5f;
    [SerializeField] private LayerMask _pushingLayer; 
    [SerializeField] private float _intRadius = 0.1f;
    private bool _isGrounded = false;
    private bool _isInteractable = false;
    private bool _isPushing = false;

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

    public bool IsPushing()
    {
        _pushingRay = new Ray(_interactOrigin.position, transform.forward);

        return _isPushing = Physics.Raycast(_pushingRay, out _pushingHit, _pushingRayDistance, _pushingLayer);
    }

    public void Interact()
    {
        _interactRay = new Ray(_interactOrigin.position, transform.forward);

        if (Physics.SphereCast(_interactRay, _intRadius, out _interactHit, _interactRayDistance, _interactLayer))
        {
            if (_interactHit.collider.TryGetComponent(out IInteractable interact))
            {
                interact.Interact();
            }
            /*else if (_interactHit.collider.TryGetComponent(out IPushable pushable))
            {
                pushable.Pushing();
            }*/
        }
    }

    public void Pushing()
    {
        _pushingRay = new Ray(_interactOrigin.position, transform.forward);

        if (Physics.SphereCast(_pushingRay, _intRadius, out _pushingHit, _pushingRayDistance, _pushingLayer))
        {
            if (_pushingHit.collider.TryGetComponent(out IPushable pushable))
            {
                pushable.Pushing();
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

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_interactHit.point, _intRadius);

        Gizmos.color = _isPushing ? Color.green : Color.red;
        Gizmos.DrawLine(_pushingRay.origin, _pushingRay.origin + _pushingRay.direction * _pushingRayDistance);
    }
}