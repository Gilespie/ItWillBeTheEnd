 using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class CameraZone : MonoBehaviour
{
    [SerializeField] CameraPointFollow _camera;
    [SerializeField] Transform _cameraPos;
    [SerializeField] Transform _cameraView;

    [SerializeField] bool _isStatic = false;
    [SerializeField] bool _isStaticPos = false;
    [SerializeField] bool _isOtherTargetView = false;
    [SerializeField] bool _isZPosition = false;
    [SerializeField] bool _isOnce = false;
    bool _alreadyTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (_isOnce && _alreadyTriggered) return;

        if (other.GetComponent<Player>() != null)
        {
            if (_isStatic)
            {
                _camera.SetPointView(_cameraPos);
                _camera.SetTargetView(_cameraView);
            }
            if(_isStaticPos)
            {
                _camera.SetPointView(_cameraPos);
            }
            if (_isOtherTargetView)
            {
                _camera.SetTargetView(_cameraView);
            }
            if(_isZPosition)
            {
                _camera.SetZPos();
            }
            if (_isOnce)
                _alreadyTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            _camera.ResetToFollow();
        }
    }
}