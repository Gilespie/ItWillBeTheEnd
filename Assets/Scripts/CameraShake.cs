using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("Shake Intensity")]
    [SerializeField, Range(1f, 10f)] float _force = 5f;
    [SerializeField] float _shakeDuration = 1.5f;
    [SerializeField] float _speedTransition = 0.1f;
    Vector3 _shakeOffset;
    [SerializeField]CameraPointFollow _cameraPoint;

    /*void Start()
    {
        _cameraPoint = GameManager.Instance.CameraPoint;
        Debug.Log(_cameraPoint + " camera from gamemanager");
    }*/

    public void ActiveShake()
    {
        StartCoroutine(ShakeCamera(_force, _shakeDuration));
    }

    IEnumerator ShakeCamera(float force, float duration)
    {
        _shakeOffset = Vector3.zero;
        float elapsed = 0.0f;
        
        while (elapsed < duration)
        {
            Vector3 offset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * force;

            _shakeOffset = Vector3.Lerp(_shakeOffset, offset, _speedTransition);

            _cameraPoint.SetShakeOffset(_shakeOffset);

            elapsed += Time.fixedDeltaTime;

            yield return null;
        }

        _cameraPoint.SetShakeOffset(Vector3.zero);
    }
}