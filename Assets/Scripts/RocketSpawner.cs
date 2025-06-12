using UnityEngine;

public class RocketSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _rocketPrefab;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private float _timeToSpawn = 3f;
    private Transform[] _targets;
    private int _randomIndex = 0;

    private void OnEnable()
    {
        _targets = GetComponentsInChildren<Transform>();
        InvokeRepeating(nameof(OnAirAttack), 0f, _timeToSpawn);    
    }

    private void OnAirAttack()
    {
        _randomIndex = Random.Range(2, _targets.Length);
        GameObject rocket = Instantiate(_rocketPrefab, _spawnPosition.position, Quaternion.identity);
        rocket.GetComponent<Rocket>().SetTarget(_targets[_randomIndex]);
    }
}