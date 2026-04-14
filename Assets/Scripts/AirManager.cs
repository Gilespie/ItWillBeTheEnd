using UnityEngine;
using UnityEngine.Rendering;

public class AirManager : MonoBehaviour
{
    [SerializeField] private float _maxOxygen = 10f;
    [SerializeField] private AudioLowPassFilter _filter;
    [SerializeField] private Collider _headCollider;
    [SerializeField]private AudioSource _audioSource;
    private Volume _currentUnderwaterVolume;
    private WaterZone _currentWaterZone;
    private bool _isUnderwater = false;
    private float _currentOxygen = 0;
    public float CurrentOxygen => _currentOxygen;

    private void Start()
    {
        _currentOxygen = _maxOxygen;
    }

    private void Update()
    {
       /* if (_isUnderwater)
        {
            _currentOxygen -= Time.deltaTime;

            if (_currentOxygen <= 0)
            {
                EventManager.Trigger(EventType.OnFinishOxygen);
            }
        }*/

        if (_currentWaterZone == null) return;

        bool headUnderwater = _headCollider.bounds.center.y < _currentWaterZone.BoundY;

        if (headUnderwater && !_isUnderwater)
        {
            EnterWater();
        }
        else if (!headUnderwater && _isUnderwater)
        {
            ExitWater();
        }

        if (_isUnderwater)
        {
            _currentOxygen -= Time.deltaTime;

            if (_currentOxygen <= 0)
            {
                EventManager.Trigger(EventType.OnFinishOxygen);
            }
        }
    }

    public void SetVolume(Volume volume)
    {
        _currentUnderwaterVolume = volume;
    }

    public void SetWaterZone(WaterZone zone)
    {
        _currentWaterZone = zone;
    }

    public void EnterWater()
    {
        _isUnderwater = true;

        if (_currentUnderwaterVolume != null)
            _currentUnderwaterVolume.enabled = true;

        _filter.enabled = true;
        _audioSource.enabled = true;
    }

    public void ExitWater()
    {
        _isUnderwater = false;
        _currentOxygen = _maxOxygen;

        if (_currentUnderwaterVolume != null)
            _currentUnderwaterVolume.enabled = false;

        _currentUnderwaterVolume = null;

        _filter.enabled = false;
        _audioSource.enabled = false;
    }
}