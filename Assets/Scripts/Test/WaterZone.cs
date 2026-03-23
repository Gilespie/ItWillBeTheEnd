using System.Collections;
using UnityEngine;

public class WaterZone : MonoBehaviour
{
    [Header("Water Settings")]
    [SerializeField] private float _dragInWater = 10f;
    [SerializeField] private float _dragInAir = 1f;
    [SerializeField] private float slowDuration = 1.0f;
    [SerializeField] private float slowFallPower = 8f;

    [Header("Splash Effect")]
    [SerializeField] private GameObject _splashPrefab;

    private Collider _collider;
    public static float _boundY;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _boundY = _collider.bounds.max.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        SpawnSplashes(other.transform.position);

        if (other.TryGetComponent(out Player player))
        {
            player.SetInWaterZone(true);

            if (player.TryGetComponent(out Rigidbody rb))
            {
                StartCoroutine(SlowFallInWater(rb));
                //rb.drag = _dragInWater;
            }
        }
        else if (other.TryGetComponent(out Rigidbody rb))
        {
            rb.linearDamping = _dragInWater;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == null) return;

        if (other.TryGetComponent(out Player player))
        {
            player.SetInWaterZone(false);

            if (player.TryGetComponent(out Rigidbody rb))
            {
                rb.linearDamping = _dragInAir;
            }
        }
        else if (other.TryGetComponent(out Rigidbody rb))
        {
            rb.linearDamping = _dragInAir;
        }
    }

    private void SpawnSplashes(Vector3 pos)
    {
        if (_splashPrefab != null)
        {
            GameObject splash = Instantiate(_splashPrefab, pos, _splashPrefab.transform.rotation);
            splash.GetComponent<ParticleSystem>().Play();
        }
    }

    private IEnumerator SlowFallInWater(Rigidbody rb)
    {
        float timer = 0f;
        Vector3 initialVelocity = rb.linearVelocity;

        while (timer < slowDuration)
        {
            timer += Time.deltaTime;
            float t = timer / slowDuration;

            float newY = Mathf.Lerp(initialVelocity.y, 0f, t * slowFallPower * Time.deltaTime);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, newY, rb.linearVelocity.z);

            yield return null;
        }

        Vector3 finalVelocity = rb.linearVelocity;
        finalVelocity.y = 0f;
        rb.linearVelocity = finalVelocity;
    }
}