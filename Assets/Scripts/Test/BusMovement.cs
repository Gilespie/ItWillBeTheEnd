using UnityEngine;

public class BusMovement : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _speed = 5f;
    [SerializeField] Transform _targetPos;
    [SerializeField] float _stopDistance = 1f;
    bool _isMoving = false;
    float _sqrtDistance = 0f;

    private void Update()
    {
        _sqrtDistance = (_targetPos.position - transform.position).sqrMagnitude;
    }
    void FixedUpdate()
    {
        if (!_isMoving) return;


        if (_sqrtDistance < _stopDistance * _stopDistance)
        {
            StopMoving();
            return;
        }


        _rb.MovePosition(_rb.position + (_targetPos.position - transform.position).normalized * _speed * Time.fixedDeltaTime);
    }

    public void StartMoving()
    {
        _isMoving = true;
    }

    public void StopMoving()
    {
        _isMoving = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Character character))
        {
            character.InstantKill();

            Rigidbody[] rbs = collision.gameObject.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rb in rbs)
            {
                rb.AddForce(_rb.linearVelocity * 30f, ForceMode.Impulse);
            }

            StopMoving();
        }
    }
}