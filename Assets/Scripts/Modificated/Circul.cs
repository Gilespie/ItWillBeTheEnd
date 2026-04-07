using UnityEngine;

public class Circul : MonoBehaviour
{
    [SerializeField] float _speedRotation = 1f;

    void Update()
    {
        transform.Rotate(Vector3.right * _speedRotation * Time.deltaTime);
    }
}