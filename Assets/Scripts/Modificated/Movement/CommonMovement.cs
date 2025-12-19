using UnityEngine;

public class CommonMovement : MovementAdvance
{
    public override void Advance(Vector3 dir)
    {
        Vector3 direction = new Vector3(dir.x, 0, dir.z);

        if(direction.sqrMagnitude > 0.001f)
        {
            _direction = direction.normalized * _speedMovement * Time.fixedDeltaTime;
            _rbMove.MovePosition(_rbMove.position + _direction);
        }
    }
}