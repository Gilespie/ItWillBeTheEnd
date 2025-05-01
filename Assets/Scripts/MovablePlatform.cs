using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _offset = 0.1f;
    [SerializeField] private LayerMask _mask;
    private int _currentIndex = 0;
    private bool _isAwait = false;

    void Start()
    {
        _currentIndex = 0;
    }

    void Update()
    {
        MoveTo();

        if(Vector3.Distance(transform.position, _waypoints[_currentIndex].position) <= _offset)
        {
            GetNextTransform();
        }
    }

    private Transform GetCurrentTransform()
    {
        return _waypoints[_currentIndex];
    }

    private Transform GetNextTransform()
    {
        _currentIndex++;

        if(_currentIndex > _waypoints.Length - 1) _currentIndex = 0;

        return _waypoints[_currentIndex];
    }

    private void MoveTo()
    {
        Vector3 dir = (_waypoints[_currentIndex].position - transform.position).normalized;

        transform.position += dir * _speed * Time.deltaTime;
    }

   /* private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }*/
}
