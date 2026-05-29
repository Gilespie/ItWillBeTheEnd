using System.Collections;
using UnityEngine;

public class RocketSpawner : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] GameObject _rocketPrefab;
    [SerializeField] Transform _spawnPosition;
    [SerializeField] Transform[] _targets;

    [SerializeField] float _timeToSpawn = 3f;
    [SerializeField] bool _isLoop = true;

    int _randomIndex = 0;
    Coroutine _attackRoutine;

    private void OnEnable()
    {
        if (_isLoop)
        {
            _attackRoutine = StartCoroutine(AirAttackLoop());
        }
        else
        {
            FireSingleRocket();
        }
    }

    private void OnDisable()
    {
        if (_attackRoutine != null)
        {
            StopCoroutine(_attackRoutine);
        }
    }

    private IEnumerator AirAttackLoop()
    {
        while (true)
        {
            FireSingleRocket();

            yield return new WaitForSeconds(_timeToSpawn);
        }
    }

    public void FireSingleRocket()
    {
        if (_targets == null || _targets.Length == 0)
            return;

        _randomIndex = Random.Range(0, _targets.Length);

        GameObject rocket = Instantiate(_rocketPrefab, _spawnPosition.position, Quaternion.identity);

        Rocket rocketComponent = rocket.GetComponent<Rocket>();

        rocketComponent.SetTarget(_targets[_randomIndex]);
    }
}