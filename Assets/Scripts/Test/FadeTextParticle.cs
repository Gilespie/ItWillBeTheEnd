using UnityEngine;
using UnityEngine.VFX;

public abstract class FadeTextParticle : MonoBehaviour
{
    protected VisualEffect _fadeTextVFX;
    protected Collider _collider;

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider>();
        _fadeTextVFX = GetComponent<VisualEffect>();
        _fadeTextVFX.Play();
    }

    public virtual void OnTriggerStay(Collider other)
    {

    }
    
    protected void ChangeState()
    {
        _fadeTextVFX.SetBool("IsTriggered", true);
        _fadeTextVFX.Reinit();
        _fadeTextVFX.Play();
        Invoke(nameof(TurnOff), 10f);
        _collider.enabled = false;
    }

    protected virtual void TurnOff()
    {
        gameObject.SetActive(false);
    }
}