using UnityEngine;

public class SlopeCharacterRotator : CharacterRotator
{
    [SerializeField] private Transform _mesh;
    private SlopeRaycast _slopeRaycast;

    public void Initialize(Rigidbody rb, SlopeRaycast slopeRaycast)
    {
        base.Initialize(rb);
        _slopeRaycast = slopeRaycast;
    }

    public override void Rotate(Vector3 dir)
    {
        if (!_slopeRaycast.IsRaycasting(-Vector3.up))
        {
            base.Rotate(dir);
            return;
        }

        Vector3 slopeNormal = _slopeRaycast.Normal;
        Vector3 velocity = _rb.linearVelocity;

        if (velocity.sqrMagnitude < 0.01f) return;

        Vector3 adjustedForward = Vector3.ProjectOnPlane(velocity, slopeNormal).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(adjustedForward, slopeNormal);

        _mesh.rotation = Quaternion.Slerp(
            _mesh.rotation,
            targetRotation,
            Time.fixedDeltaTime * 10f
        );
    }
}