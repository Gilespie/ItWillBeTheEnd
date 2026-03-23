using System;
using UnityEngine;
using UnityEngine.Rendering;

public class AirManager : MonoBehaviour
{
    [SerializeField] private float _maxOxygen = 10f;
    [SerializeField] private AudioLowPassFilter _filter;
    [SerializeField] private Collider _headCollider;
    [SerializeField] private Volume _underwatervolume;
    private bool _isUnderwater = false;
    private float _currentOxygen = 0;
    public float CurrentOxygen => _currentOxygen;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _currentOxygen = _maxOxygen;

        if(_underwatervolume != null)
        _underwatervolume.enabled = false;
    }

    private void Update()
    {
        if (_isUnderwater)
        {
            _currentOxygen -= Time.deltaTime;

            if (_currentOxygen <= 0)
            {
                EventManager.Trigger(EventType.OnFinishOxygen);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WaterZone zone))
        {
            ChangeBoolState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out WaterZone zone))
        {
            ChangeBoolState(false);
        }
    }

    public void ChangeBoolState(bool state)
    {
        if (!state) { _currentOxygen = _maxOxygen; }

        _isUnderwater = state;

        if(_underwatervolume != null)
        _underwatervolume.enabled = state;

        _filter.enabled = state;
        _audioSource.enabled = state;
    }
}