using UnityEngine;


public class FallingRock : MonoBehaviour
{
    [SerializeField] private float _damage = 60f;
    private bool _isOnce = false;
    private float _velocityMagnitude;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isOnce) return;

        if (collision.gameObject.TryGetComponent(out Player player))
        {
            _velocityMagnitude = _rb.velocity.magnitude;
            _damage *= _velocityMagnitude;
            player.TakeDamage(_damage);
            _isOnce = true;
        }
    }
}