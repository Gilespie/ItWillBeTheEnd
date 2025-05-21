using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string _boolName = "isOpened";
    [SerializeField] private AudioClip _audioClip;
    private AudioSource _audioSource;
    private Animator _animator;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponentInChildren<Animator>();
    }

    public void ChangeState(bool state)
    {
        _animator.SetBool(_boolName, state);
    }

    public void PlaySFX()
    {
        _audioSource.clip = _audioClip;
        _audioSource.Play();
    }
}