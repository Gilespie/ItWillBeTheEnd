using UnityEngine;

public abstract class MovementAdvance : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] protected float _speedMovement;

    [Header("Acceleration")]
    [SerializeField] protected float _acceleration = 10f;
    [SerializeField] protected float _deceleration = 10f;

    [Header("Jump")]
    [SerializeField] protected float _jumpForce = 5f;

    protected Rigidbody _rb;
    protected Vector3 _direction;
    protected Vector3 _lastDirection;

    protected float _currentSpeed;
    public float CurrentSpeed => _currentSpeed;

    public virtual void Initialize(Rigidbody move)
    {
        _rb = move;
    }

    public abstract void Advance(Vector3 dir, Vector3 externalVelocity);

    public virtual void SetSpeed(float speed)
    {
        _currentSpeed = speed;
    }

    public virtual void Jump()
    {
        /* _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
         _rb.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);*/
        /*Vector3 vel = _rb.linearVelocity;
        vel.y = 0f;
        _rb.linearVelocity = vel;*/
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
    }
}