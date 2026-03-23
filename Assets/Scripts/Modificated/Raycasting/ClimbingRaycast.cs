using UnityEngine;

public class ClimbingRaycast : AbstractRaycast
{
    [SerializeField] float _maxHeight = 2f;

    public override bool IsRaycasting(Vector3 direction)
    {
        _ray = new Ray(_originPoint.position, direction);

        if (Physics.Raycast(_ray, out _hit, _rayDistance, _affectedLayer))
        {
            return _isHitted = true;
        }

        return _isHitted = false;
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = _isHitted ? Color.green : Color.red;
        Gizmos.DrawLine(_ray.origin, _ray.origin + _ray.direction * _rayDistance);

        if (_isHitted)
        {
            Gizmos.DrawLine(_hit.point, _hit.point + Vector3.up * _maxHeight);
        }
    }
}