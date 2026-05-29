using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class ExplosionTest : MonoBehaviour
{
    [SerializeField] VisualEffect[] effects;
    [SerializeField] float _delay = 10f;

    private void OnEnable()
    {
        PlayEffects();
        StartCoroutine(ResetRoutine());
    }

    private void OnDisable()
    {
        StopEffects();
    }

    [ContextMenu("Play")]
    public void PlayEffects()
    {
        foreach (var effect in effects)
        {
            effect.Play();
        }
    }

    private void StopEffects()
    {
        foreach (var effect in effects)
        {
            effect.Stop();
        }
    }

    IEnumerator ResetRoutine()
    {
        yield return new WaitForSeconds(_delay);
        gameObject.SetActive(false);
        yield return null;
    }
}