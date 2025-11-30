using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class CameraZone : MonoBehaviour
{
    [SerializeField] private CameraPointFollow _camera;
    [SerializeField] private Transform _cameraPos;
    [SerializeField] private Transform _cameraView;

    [SerializeField] bool _isStatic = false;
    [SerializeField] bool _isStaticPos = false;
    [SerializeField] bool _isOtherTargetView = false;
    [SerializeField] bool _isOnce = false;
    bool _alreadyTriggered;

    [SerializeField] Vector3 _staticPosition;
    public Vector3 staticPosition => _staticPosition;

    [SerializeField] private float _minX, _maxX, _minY, _maxY;
    public float MinX => _minX;
    public float MaxX => _maxX;
    public float MinY => _maxY;
    public float MaxY => _maxY;

    private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();

        CalculateBounds();
        CalculateStaticPoint();
    }
    
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
            if (_isOnce)
                _alreadyTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (_isOnce && _alreadyTriggered) return;
        
        if (other.GetComponent<Player>() != null)
        {
            _camera.ResetToFollow(); // ← возвращаемся к следованию за игроком
        }
    }

    private void  CalculateBounds()
    {
        Bounds b = _boxCollider.bounds;

        float halfWidth = b.extents.x;
        float halfHeight = b.extents.y;
        float halfDepth = b.extents.z;
        Vector3 center = b.center;
        
        _minX = center.x - halfWidth;
        _maxX = center.x + halfWidth;
        _minY = center.y - halfHeight;
        _maxY = center.y + halfHeight;
    }

    private void CalculateStaticPoint()
    {
        Bounds b = _boxCollider.bounds;
        Vector3 center = b.center;

        _staticPosition = center;
    }
}