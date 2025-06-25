using System.Collections;
using UnityEngine;

public class RocketSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _rocketPrefab;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private float _timeToSpawn = 3f;
    [SerializeField] private bool _isLoop = true;
    private Transform[] _targets;
    private int _randomIndex = 0;

    private void OnEnable()
    {
        _targets = GetComponentsInChildren<Transform>();
        StartCoroutine(OnAirAttack());
    }

    private void OnDisable()
    {
        StopCoroutine(OnAirAttack());
    }


    private IEnumerator OnAirAttack()
    {
        while(_isLoop)
        {
            _randomIndex = Random.Range(2, _targets.Length);
            GameObject rocket = Instantiate(_rocketPrefab, _spawnPosition.position, Quaternion.identity);
            rocket.GetComponent<Rocket>().SetTarget(_targets[_randomIndex]);
            yield return new WaitForSeconds(_timeToSpawn);

            yield return null;
        }

        yield return null;
    }
}