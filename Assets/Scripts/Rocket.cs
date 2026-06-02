using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class Rocket : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _speedFly = 55f;
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _damage = 500f;
    [SerializeField] private float _forceExplosion = 50f;
    [SerializeField] private float _forceUpExplosion = 5f;
    [SerializeField] private LayerMask _player;

    [SerializeField] float _timeToDeactivate = 5f;

    [Header("References")]
    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] VisualEffect _explosionVFX;
    [SerializeField] AudioClip _flyClip;
    [SerializeField] GameObject _mesh;
    [SerializeField] Collider _col;
    [SerializeField] ParticleSystem _trailParticle;
    private Transform _target;
    private Rigidbody _rb;
    private AudioSource _audiosource;
    private Vector3 _direction;
    [SerializeField] bool _isVFX;
    bool _isExploded;

    private void Awake()
    {
        _audiosource = GetComponent<AudioSource>();
        _audiosource.clip = _flyClip;
        _audiosource.Play();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_isExploded || _target == null)
            return;

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

            if(!_isVFX)
                Instantiate(_explosionPrefab, collision.contacts[0].point, _explosionPrefab.transform.rotation);
            else
                Instantiate(_explosionVFX, collision.contacts[0].point, _explosionVFX.transform.rotation);
            //CameraShake.Instance.ActiveShake();

            _mesh.SetActive(false);
            _col.enabled = false;
            _trailParticle.Stop();

            _isExploded = true;
            StartCoroutine(WaitParticles());
            //Destroy(gameObject);
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
                dest.InstantKill();

                Rigidbody[] rb = col.GetComponentsInChildren<Rigidbody>();

                foreach(Rigidbody rb2 in rb)
                {
                    rb2.AddExplosionForce(_forceExplosion, centre, _radius, _forceUpExplosion, ForceMode.Impulse);
                }
            }
        }
    }

    IEnumerator WaitParticles()
    {
        yield return new WaitForSeconds(_timeToDeactivate);
        gameObject.SetActive(false);
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }*/
}