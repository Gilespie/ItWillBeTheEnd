using System.Collections;
using UnityEngine;

public class FallablePlatform : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _delay = 1.5f;
    private Rigidbody _rigidbody;
    private Coroutine _currentRoutine = null;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_currentRoutine != null) return;


        if(_currentRoutine == null && collision.gameObject.TryGetComponent(out _player))
        {
            StartCoroutine(ActivateKinemticRoutine());
        }
    }

    private IEnumerator ActivateKinemticRoutine()
    {
        yield return new WaitForSeconds(_delay);
        _rigidbody.isKinematic = false;
        yield return null;
    }
}