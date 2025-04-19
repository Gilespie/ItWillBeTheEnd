using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;

    public void TakeImpulse(float magnitude)
    {
        _rb.AddForce(_rb.position *  magnitude, ForceMode.Impulse);
    }
}