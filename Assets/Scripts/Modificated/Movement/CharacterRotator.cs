using UnityEngine;

public class CharacterRotator : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float _speedRotDefault = 10f;
    private Rigidbody _rb;

    public virtual void Initialize(Rigidbody move)
    {
        _rb = move;
    }

    public virtual void Rotate(Vector3 dir)
    {
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(dir);

        Quaternion smoothRotation = Quaternion.Slerp(_rb.rotation, targetRotation, _speedRotDefault * Time.fixedDeltaTime);

        _rb.MoveRotation(smoothRotation);
    }
}
