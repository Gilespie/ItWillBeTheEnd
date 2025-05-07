using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent _event;
    [SerializeField] private InteractMode _interactMode = InteractMode.Once;
    [SerializeField] private Color _colorOn;
    [SerializeField] private Color _colorOff;
    [SerializeField] private AudioClip _sfxOn;
    [SerializeField] private AudioClip _sfxOff;
    [SerializeField] private Light _light;
    [SerializeField] private AudioSource _audioSource;
    private bool _isActive = false;
    private int _count = 0;

    private void Start()
    {
        _light.color = _colorOff;
    }

    public void Interact()
    {
        switch (_interactMode)
        {
            case InteractMode.Once:
                if (_count > 0) return;
                ActivateButton();
                PlaySound(_sfxOn, _sfxOff);
                break;

            case InteractMode.Toggle:
                ActivateButton();
                PlaySound(_sfxOn, _sfxOff);
                break;
        } 
    }

    private void ActivateButton()
    {
        _isActive = !_isActive;
        _light.color = _isActive ? _colorOn : _colorOff;
        _event?.Invoke();
        _count++;
    }

    private void PlaySound(AudioClip clip1, AudioClip clip2)
    {
        _audioSource.clip = _isActive ? clip1 : clip2;
        _audioSource.Play();
    }
}