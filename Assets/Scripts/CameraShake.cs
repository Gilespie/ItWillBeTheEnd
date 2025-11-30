using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    [Header("Shake Intensity")]
    [SerializeField, Range(1f, 10f)] private float _force = 5f;
    [SerializeField] private float _shakeDuration = 1.5f;
    [SerializeField] private float _speedTransition = 0.1f;
    private Vector3 _shakeOffset;
    private CameraPointFollow _cameraPoint;

    private void Start()
    {
        _cameraPoint = GameManager.Instance.CameraPoint;
    }

    public void ActiveShake()
    {
        StartCoroutine(ShakeCamera(_force, _shakeDuration));
    }

    public IEnumerator ShakeCamera(float force, float duration)
    {
        _shakeOffset = Vector3.zero;
        float elapsed = 0.0f;
        
        while (elapsed < duration)
        {
            Vector3 offset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * force;

            _shakeOffset = Vector3.Lerp(_shakeOffset, offset, _speedTransition);

            _cameraPoint.SetShakeOffset(_shakeOffset);

            elapsed += Time.deltaTime;

            yield return null;
        }

        _cameraPoint.SetShakeOffset(Vector3.zero);
    }
}