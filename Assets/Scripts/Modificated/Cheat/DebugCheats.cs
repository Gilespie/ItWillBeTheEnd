using UnityEngine;

public class DebugCheats : SingletonBase<DebugCheats>
{
    [SerializeField] bool _isDebugMode = false;
    [SerializeField] Transform[] _subdivisions;
    [SerializeField] private Transform _target;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            _isDebugMode = !_isDebugMode;
            Debug.Log($"Debug mode: {_isDebugMode}");
        }

        if(_isDebugMode)
        {
            for (int i = 0; i < _subdivisions.Length; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    _target.position = _subdivisions[i].position;
                }
            }
        }
    }
}