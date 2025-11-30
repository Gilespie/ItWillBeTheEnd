using UnityEngine;

public class TeleportForSubdivisions : MonoBehaviour
{
    [SerializeField] Transform[] _subdivisions;
    [SerializeField] private Transform _target;
    private void Update()
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