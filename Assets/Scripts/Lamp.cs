using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    private Light _light;

    private void Start()
    {
        _light = GetComponentInChildren<Light>();
    }

    public void TakeImpulse(float magnitude)
    {
        _rb.AddForce(_rb.position *  magnitude, ForceMode.Impulse);
    }

    public void ActivateLamp(bool active)
    {
        if (active)
        {
            _light.enabled = true;
        }
        else
        {
            _light.enabled = false;
        }
            
    }
}