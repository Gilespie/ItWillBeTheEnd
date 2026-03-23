using UnityEngine;

public class ClimbMovement : MovementAdvance
{
    public override void Advance(Vector3 dir)
    {
        Vector3 direction = new Vector3(dir.x,0,0);

        if (direction.sqrMagnitude > 0.001f)
        {
            _direction = direction.normalized * _speedMovement * Time.fixedDeltaTime;
            _rb.MovePosition(_rb.position + _direction);
        }
    }
}