using UnityEngine;

public class SwimmMovement : MovementAdvance
{
    public override void Advance(Vector3 dir)
    {
        Vector3 direction = dir;

        if (direction.sqrMagnitude > 0.001f)
        {
            _direction = direction.normalized * _speedMovement * Time.fixedDeltaTime;
            _rbMove.MovePosition(_rbMove.position + _direction);
        }
    }
}