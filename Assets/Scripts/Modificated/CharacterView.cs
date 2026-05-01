using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField] ParticleSystem _bloodVFX;
    [SerializeField] ParticleSystem[] _bubbleVFX;
    [SerializeField] ParticleSystem _dustVFX;

    public void PlayBloodVFX()
    {
        _bloodVFX.Play();
    }

    public void PlayBubbleVFX(bool value)
    {
        if(value)
        {
            foreach (var particle in _bubbleVFX)
            {
                particle.Play();
            }
        }
        else
        {
            foreach (var particle in _bubbleVFX)
            {
                particle.Stop();
            }
        }
    }

    public void PlayDustVFX(bool value)
    {
        if (value)
        {
            _dustVFX.Play();
        }
        else
        {
            _dustVFX.Stop();
        }

    }
}