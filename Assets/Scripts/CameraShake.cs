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
            SceneManager.sceneLoaded += OnSceneLoaded; // Подписка
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded; // Отписка
        }
    }

    [Header("Shake Intensity")]
    [SerializeField, Range(1f, 10f)] private float _force = 5f;
    [SerializeField] private float _shakeDuration = 1.5f;
    [SerializeField] private float _speedTransition = 0.1f;
    private Vector3 _shakeOffset;
    private CameraFollower _camera;

    private void Start()
    {
        _camera = GameManager.Instance.Camera;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryAssignCamera();
    }

    private void TryAssignCamera()
    {
        if (GameManager.Instance != null)
        {
            _camera = GameManager.Instance.Camera;
        }
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

            _camera.SetShakeOffset(_shakeOffset);

            elapsed += Time.deltaTime;

            yield return null;
        }

        _camera.SetShakeOffset(Vector3.zero);
    }

}
