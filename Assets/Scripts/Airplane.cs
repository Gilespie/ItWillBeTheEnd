using UnityEngine;
using UnityEngine.UIElements;

public class Airplane : MonoBehaviour
{
    [SerializeField] private Lights[] _lights = new Lights[4];
    //[SerializeField] private GameObject _mesh;
    [SerializeField] private ParticleSystem _dustParticle;
    [SerializeField] private ParticleSystem _dustShockParticle;
    [SerializeField] private GameObject _explosivePrefab;
    [SerializeField] private bool _isLanding = false;
    [SerializeField] private bool _isCrashing = false;
    [SerializeField] private bool _isCrashing2 = false;
    private string _boolCrashName = "isCrashing";
    private string _boolCrash2Name = "isCrashing2";
    private string _boolLandName = "isLanding";
    private Animator _animator;
    private AudioSource _audiosource;

    private void Awake()
    {
        _audiosource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _lights = GetComponentsInChildren<Lights>();

        if (_isLanding)
        {
            _animator.SetTrigger(_boolLandName);
        }
        else if (_isCrashing)
        {
            _animator.SetTrigger(_boolCrashName);
        }
        else if(_isCrashing2)
        {
            _animator.SetTrigger(_boolCrash2Name);
        }

       // _mesh.SetActive(false);
    }

    public void CrashPlane()
    {
        _animator.enabled = true;
       // _mesh.SetActive(true);
    }

    public void TurnOffLights()
    {
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].TurnOffLight();
        }
    }

    public void Explote()
    { 
        Instantiate(_explosivePrefab, transform.position, _explosivePrefab.transform.rotation);
    }

    public void Dust()
    {
        Instantiate(_dustParticle, new(transform.position.x, transform.position.y - 11f, transform.position.z), _dustParticle.transform.rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Character player))
        {
            player.InstantKill();
        }
    }
}
