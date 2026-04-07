using UnityEngine;

public class PushMovement : MovementAdvance
{
    public override void Advance(Vector3 dir)
    {
        PushMove(dir);
    }

    public void PushMove(Vector3 dir)
    {
        PushableBox box = _rb.GetComponentInParent<Character>().CurrentBox;

        if (box == null) return;

        _direction = new Vector3(dir.x, 0, dir.z);

        float targetSpeed = _direction.sqrMagnitude > 0.01f ? _speedMovement : 0f;

        float accel = targetSpeed > _currentSpeed ? _acceleration : _deceleration;

        _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, accel * Time.fixedDeltaTime);

        Vector3 velocity = _direction.normalized * _currentSpeed;

        _rb.MovePosition(_rb.position + velocity * Time.fixedDeltaTime);

        Rigidbody boxRb = box.GetComponent<Rigidbody>();
        boxRb.MovePosition(boxRb.position + velocity * Time.fixedDeltaTime);
    }
}