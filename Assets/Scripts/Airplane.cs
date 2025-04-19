using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Airplane : MonoBehaviour
{
    [SerializeField] private Lights[] _lights = new Lights[4];
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private GameObject _explosivePrefab;
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
    }

    public void CrashPlane()
    {
        _audiosource.Play();
        _animator.enabled = true;
        Invoke("Explote", 8f);
    }

    public void TurnoffLights()
    {
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].TurnOffLight();
        }
    }

    public void Explote()
    {
        Instantiate(_explosivePrefab, new Vector3(191.3f, 0, 68.5f), _explosivePrefab.transform.rotation);
    }
}
