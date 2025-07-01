using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Light[] _lights;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void TurnOff()
    {
        Debug.Log("");
        _audioSource.Play();

        foreach (Light light in _lights)
        {
            light.gameObject.SetActive(false);
        }
    }
}