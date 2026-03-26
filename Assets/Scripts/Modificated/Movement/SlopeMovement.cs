using UnityEngine;

public class SlopeMovement : MovementAdvance
{
    SlopeRaycast _slopeRaycast;

    public void Initialize(Rigidbody move, SlopeRaycast raycast)
    {
        base.Initialize(move);
        _slopeRaycast = raycast;
    }
    public override void Advance(Vector3 dir)
    {
        if (!_slopeRaycast.IsRaycasting(-Vector3.up)) return;

        Vector3 slopeNormal = _slopeRaycast.Normal;
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, slopeNormal).normalized;

        _direction = forward * dir.z;
        _rb.MovePosition(_rb.position + _direction.normalized * _speedMovement * Time.fixedDeltaTime);
    }
}