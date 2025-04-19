using UnityEngine;

[ExecuteAlways]
public class Raycasting : MonoBehaviour
{
    [Header("Rays")]
    [SerializeField] private Ray _groundRay;
    [SerializeField] private Ray _groundInteractable;

    [Header("Settings")]
    [SerializeField] private float _groundRayDistance = 0.2f;
    [SerializeField] private float _yPosOffset = 0.2f;
    [SerializeField] private float _interactRayDistance = 0.2f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _interactLayer;
    private Vector3 _groundRayOffset;
    private Vector3 _interactRayOffset;
    private bool _isGrounded = false;

    public bool IsGrounded()
    {
        _groundRayOffset = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);

        _groundRay = new Ray(_groundRayOffset, -transform.up);

        return _isGrounded = Physics.Raycast(_groundRay, _groundRayDistance, _groundLayer);
    }

    private void OnDrawGizmos()
    {
        if (_isGrounded)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        
        Gizmos.DrawLine(_groundRay.origin, _groundRay.origin + _groundRay.direction * _groundRayDistance);
    }
}