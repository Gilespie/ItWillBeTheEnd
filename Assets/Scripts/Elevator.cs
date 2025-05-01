using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float _elapsedTime = 0f;
    [SerializeField] private float _timeToPoint = 0f;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Transform[] _points;
    private int _currentIndex = 0;
    private int _previousIndex = 0;
    private float _distanceToPoint = 0f;
    private float _elapsedPercetage = 0f;
    //private float _offset = 0.1f;
    //private Rigidbody _rb;


    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;

        _distanceToPoint = Vector3.Distance(GetPreviousPoint().position, GetNextPoint().position);

        _timeToPoint = _distanceToPoint/ _moveSpeed;

        _elapsedPercetage = _elapsedTime / _timeToPoint;

        _elapsedPercetage = Mathf.SmoothStep(0, 1, _elapsedPercetage);

        Vector3 dir = Vector3.Lerp(GetPreviousPoint().position, GetNextPoint().position, _elapsedPercetage);

        transform.position = dir;

        if(_elapsedPercetage >= 1)
        {
            Debug.Log("Next");
            GetNextPoint();
            _elapsedTime = 0f;
        }
    }

    private Transform GetCurrentPoint()
    {
        return _points[ _currentIndex ];
    }

    private Transform GetNextPoint()
    {
        _currentIndex++;

        if (_currentIndex > _points.Length - 1) _currentIndex = 0;

        return _points[_currentIndex];
    }

    private Transform GetPreviousPoint()
    {
        _currentIndex--;

        if (_currentIndex < 0) _currentIndex = _points.Length - 1;

        return _points[_currentIndex];
    }
}