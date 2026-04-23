using UnityEngine;
using UnityEngine.VFX;

public class CharacterView : MonoBehaviour
{
    [SerializeField] ParticleSystem _bloodVFX;

    public void PlayBloodVFX()
    {
        _bloodVFX.Play();
    }
}