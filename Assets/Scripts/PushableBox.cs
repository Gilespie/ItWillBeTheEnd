using UnityEngine;

public class PushableBox : MonoBehaviour
{
    [SerializeField] private float _damage = 60f;

    [SerializeField]private Player _player;
    bool _canPushing = false;
    private Rigidbody _rb;
    //private bool _isOnce = false;
    private float _velocityMagnitude;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out _player))
        {
            _canPushing = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out _player))
        {
            if (_canPushing && _player.IsPushing)
            {
                StartMoving();
            }
            else
            {
                StopMoving();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out _player))
        {
            _canPushing = false;
        }
    }

    public void StartMoving()
    {
        transform.parent = _player.transform;
        _rb.isKinematic = true;
    }

    public void StopMoving()
    {
        transform.parent = null;
        _rb.isKinematic = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        //if (_isOnce) return;

        if (collision.gameObject.TryGetComponent(out _player))
        {
            _velocityMagnitude = _rb.velocity.magnitude;
            _damage *= _velocityMagnitude;
            _player.TakeDamage(_damage);
            //_isOnce = true;
        }
    }
}