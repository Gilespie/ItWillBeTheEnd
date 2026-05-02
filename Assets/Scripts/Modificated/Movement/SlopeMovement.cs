using UnityEngine;

public class SlopeMovement : MovementAdvance
{
    [SerializeField] SlopeRaycast _slopeRaycast;
    [SerializeField] float _maxSlideSpeed = 6f;

    public override void Advance(Vector3 dir)
    {
        if (!_slopeRaycast.IsRaycasting(-Vector3.up)) return;

        Vector3 slopeNormal = _slopeRaycast.Normal;

        _direction = transform.forward * dir.z;
        _rb.MovePosition(_rb.position + _direction.normalized * _speedMovement * Time.fixedDeltaTime);
    }
}