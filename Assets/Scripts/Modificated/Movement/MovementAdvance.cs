using UnityEngine;

public abstract class MovementAdvance : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] protected float _speedMovement;

    [Header("Acceleration")]
    [SerializeField] protected float _acceleration = 10f;
    [SerializeField] protected float _deceleration = 10f;

    [Header("Rotation")]
    [SerializeField] protected float _speedRotDefault = 10f;

    [Header("Jump")]
    [SerializeField] protected float _jumpForce = 5f;

    protected Rigidbody _rb;
    protected Vector3 _direction;

    protected float _currentSpeed;
    public float CurrentSpeed => _currentSpeed;

    public virtual void Initialize(Rigidbody move)
    {
        _rb = move;
    }

    public abstract void Advance(Vector3 dir);

    public virtual void SetSpeed(float speed)
    {
        _currentSpeed = speed;
    }

    public virtual void Rotate(Vector3 dir)
    {
        if (dir.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(dir);

        Quaternion smoothRotation = Quaternion.Slerp(_rb.rotation, targetRotation, _speedRotDefault * Time.fixedDeltaTime);

        _rb.MoveRotation(smoothRotation);
    }

    public virtual void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Acceleration);
    }
}