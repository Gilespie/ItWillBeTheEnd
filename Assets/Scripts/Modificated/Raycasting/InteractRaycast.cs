using UnityEngine;

public class InteractRaycast : AbstractRaycast
{
    [SerializeField] float _intRadius = 0.1f;

    public new void IsRaycasting(Vector3 direction)
    {
        _ray = new Ray(_originPoint.position, direction);

        if (Physics.SphereCast(_ray, _intRadius, out _hit, _rayDistance, _affectedLayer))
        {
            if (_hit.collider.TryGetComponent(out IInteractable interact))
            {
                interact.Interact();
            }
        }
    }
}