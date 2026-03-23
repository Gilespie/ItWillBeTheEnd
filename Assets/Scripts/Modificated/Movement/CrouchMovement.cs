using UnityEngine;

public class CrouchMovement : MovementAdvance
{
    [SerializeField] CapsuleCollider _col;
    
    public override void Advance(Vector3 dir)
    {
        _col.height = 1;
        _col.center = new Vector3(_col.center.x, 0.5f, _col.center.z);

        Vector3 direction = new Vector3(dir.x, 0, dir.z);

        float targetSpeed = direction.sqrMagnitude > 0.01f ? _speedMovement : 0f;

        float accel = targetSpeed > _currentSpeed ? _acceleration : _deceleration;

        _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, accel * Time.fixedDeltaTime);

        Vector3 velocity = direction.normalized * _currentSpeed;

        _rb.MovePosition(_rb.position + velocity * Time.fixedDeltaTime);

        Rotate(direction);
    }
}