using UnityEngine;

public class ZLimiter : MonoBehaviour
{
    [Header("Z Limits")]
    [SerializeField] private float _zPosMin = -10f;
    [SerializeField] private float _zPosMax = 0f;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        CheckZPosition();
    }

    private void CheckZPosition()
    {

        Vector3 pos = _rb.position;
        pos.z = Mathf.Clamp(pos.z, _zPosMin, _zPosMax);
        _rb.MovePosition(pos);

       /* if (transform.position.z >= _zPosMax)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _zPosMax);
        }
        if (transform.position.z <= _zPosMin)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _zPosMin);
        }*/
    }
}