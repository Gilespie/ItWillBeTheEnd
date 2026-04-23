using UnityEngine;

public class CCTV : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] Transform _cameraBody;
    [SerializeField] Camera _cam;

    private void Update()
    {
        if (_target == null) return;

        _cam.transform.LookAt(_target);
        _cameraBody.transform.LookAt(_target.position);
    }
}