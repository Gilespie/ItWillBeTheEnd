using UnityEngine;

public class FireworksController : MonoBehaviour
{
    [SerializeField] AudioClip _clip;
    [SerializeField] AudioSource _source;

    void Start()
    {
        Invoke(nameof(PlaySound), 3f);
    }

    void PlaySound()
    {
        _source.PlayOneShot(_clip);
    }
}
