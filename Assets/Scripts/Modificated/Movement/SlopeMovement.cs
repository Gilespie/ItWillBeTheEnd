using UnityEngine;

public class SlopeMovement : MovementAdvance
{
    public override void Advance(Vector3 dir)
    {
        _direction = transform.forward * dir.z;
        _rb.MovePosition(_rb.position + _direction.normalized * _speedMovement * Time.fixedDeltaTime);
    }
}
