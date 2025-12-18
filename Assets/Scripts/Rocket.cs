using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Rocket : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _speedFly = 55f;
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _damage = 500f;
    [SerializeField] private float _forceExplosion = 50f;
    [SerializeField] private float _forceUpExplosion = 5f;
    [SerializeField] private LayerMask _player;

    [Header("References")]
    [SerializeField] private GameObject[] _explosions;
    [SerializeField] private AudioClip _flyClip;
    private Transform _target;
    private Rigidbody _rb;
    private AudioSource _audiosource;
    private Vector3 _direction;

    private void Awake()
    {
        _audiosource = GetComponent<AudioSource>();
        _audiosource.clip = _flyClip;
        _audiosource.Play();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _direction = (_target.position - transform.position).normalized;
        Quaternion view = Quaternion.LookRotation(_direction);

        _rb.MoveRotation(view);
        _rb.MovePosition(_rb.position + _direction * _speedFly * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider != null)
        {
            Explosion(collision.contacts[0].point);

            Instantiate(_explosions[0], collision.contacts[0].point, _explosions[0].transform.rotation);
            Instantiate(_explosions[1], collision.contacts[0].point, _explosions[1].transform.rotation);

            CameraShake.Instance.ActiveShake();

            _target.GetComponent<DecalProjector>().enabled = false;
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void Explosion(Vector3 centre)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, _player);

        foreach (Collider col in colliders)
        {
            Destructable dest = col.GetComponent<Destructable>();

            if (dest != null)
            {
                dest.TakeDamage(_damage);

                Rigidbody[] rb = col.GetComponentsInChildren<Rigidbody>();

                foreach(Rigidbody rb2 in rb)
                {
                    rb2.AddExplosionForce(_forceExplosion, centre, _radius, _forceUpExplosion, ForceMode.Impulse);
                }
            }
        }
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }*/
}