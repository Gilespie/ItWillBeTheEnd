using UnityEngine;

public abstract class MovementAdvance : MonoBehaviour
{
    [SerializeField] protected float _speedMovement;
    protected Rigidbody _rbMove;
    protected Vector3 _direction;

    public virtual void Initialize(Rigidbody move)
    {
        _rbMove = move;
    }

    public abstract void Advance(Vector3 dir);
}