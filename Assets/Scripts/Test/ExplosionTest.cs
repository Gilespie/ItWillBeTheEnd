using UnityEngine;
using UnityEngine.VFX;

public class ExplosionTest : MonoBehaviour
{
    [SerializeField] VisualEffect[] effects;


    [ContextMenu("Play")]
    public void PlayEffects()
    {
        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));

        foreach (var effect in effects)
        {
            effect.gameObject.transform.position = randomPos;
            effect.Play();
        }
    }
}