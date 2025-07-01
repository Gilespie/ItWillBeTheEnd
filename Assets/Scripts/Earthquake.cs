using UnityEngine;

public class Earthquake : MonoBehaviour
{
    [SerializeField] private AudioClip[] _clips;
    [SerializeField] private ParticleSystem[] _dusts;
    [SerializeField] private Rigidbody[] _rbs;
    [SerializeField] private float _timeToQuake = 15;
    [SerializeField] private float _force = 2f;
    private AudioSource _audioSource;
    private float _currentTimer = 0f;
    private int _randomClipIndex = 0;
    private float _randomPitch = 0f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _currentTimer = _timeToQuake;
        Shockwaves();
    }

    private void Update()
    {
        _currentTimer -= Time.deltaTime;
        
        if(_currentTimer < 0f)
        {
            _currentTimer = _timeToQuake;
            Shockwaves();
        }
    }

    private void Shockwaves()
    {
        _randomClipIndex = Random.Range( 0, _clips.Length);
        _randomPitch = Random.Range(0.5f, 1f);

        if (_audioSource != null)
        {
            _audioSource.pitch = _randomPitch;
            _audioSource.PlayOneShot(_clips[_randomClipIndex]);
        }

        foreach(var dust in _dusts)
        {
            dust.Play();
        }

        foreach(var rb in _rbs)
        {
            rb.AddForce(new Vector3(Random.Range(-1f, 1), 0f, Random.Range(-1f, 1)) * _force, ForceMode.Impulse);
        }

        CameraShake.Instance.ActiveShake();
    }
}
