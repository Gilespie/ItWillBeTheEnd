using UnityEngine;

public class SlopeMovement : MovementAdvance
{
    public override void Advance(Vector3 dir)
    {
        _direction = transform.forward * dir.z;
        _rbMove.MovePosition(_rbMove.position + _direction.normalized * _speedMovement * Time.fixedDeltaTime);
    }
}
