using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Collider[] _ragdollColliders;
    [SerializeField] private Rigidbody[] _ragdollRBs;
    [SerializeField] private Animator _animator;

    void Start()
    {
        DisableRagdoll();
    }

    public void DisableRagdoll()
    {
        foreach (var col in _ragdollColliders)
        {
            col.enabled = false;
        }

  /*      foreach (var rb in _ragdollRBs)
        {
            rb.isKinematic = false;
        }*/

        _animator.enabled = true;
    }

    public void ActivateRagdoll()
    {
        _animator.enabled = false;

        foreach (var col in _ragdollColliders)
        {
            col.enabled = true;
        }

        /*foreach (var rb in _ragdollRBs)
        {
            rb.isKinematic = false;
        }*/
    }
}