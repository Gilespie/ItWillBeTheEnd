using UnityEngine;

public class FireworksController : MonoBehaviour
{
    [SerializeField] AudioClip _clip;
    [SerializeField] AudioSource _source;

    public void StartEvent()
    {
        Invoke(nameof(PlaySound), 3f);
    }

    void PlaySound()
    {
        _source.PlayOneShot(_clip);
    }
}