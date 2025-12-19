using UnityEngine;

public class RigidbodyRotator : MonoBehaviour
{
    [SerializeField] float _speedRotDefault = 10f;
    [SerializeField] CharacterInputController _controller;
    Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Rotate(_controller.Direction);
    }

    void Rotate(Vector3 dir)
    {
        if(dir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            Quaternion smoothRotation = Quaternion.Slerp(_rb.rotation, targetRotation, _speedRotDefault * Time.fixedDeltaTime);

            _rb.MoveRotation(smoothRotation);
        }
    }
}