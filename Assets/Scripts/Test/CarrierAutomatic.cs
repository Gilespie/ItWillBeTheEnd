using UnityEngine;

public class CarrierAutomatic : MonoBehaviour
{
    [SerializeField] Animator _animator;

    public void ActivateCarrier()
    {
        _animator.SetTrigger("OnActive");
    }
}