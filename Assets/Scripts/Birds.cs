using UnityEngine;

public class Birds : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;
    private ParticleSystem.MainModule _main;

    private void Start()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _main = _particleSystem.main;
        IdleBirds(true, 0,0);
    }

    public void StartBirdEvent()
    {
        IdleBirds(false, 7,13);
        _audioSource.Play();
    }

    public void IdleBirds(bool isLoop, float startSpeedMin, float startSpeedMax)
    {
        _particleSystem.Clear();
        _main.loop = isLoop;
        _main.startSpeed = new ParticleSystem.MinMaxCurve(startSpeedMin,startSpeedMax);
        _particleSystem.Play();
    }
}