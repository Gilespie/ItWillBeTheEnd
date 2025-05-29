using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZLimiter : MonoBehaviour
{
    [Header("Z Limits")]
    [SerializeField] private float _zPosMin = -10f;
    [SerializeField] private float _zPosMax = 0f;

    private void Update()
    {
        CheckZPosition();
    }

    private void CheckZPosition()
    {
        if (transform.position.z >= _zPosMax)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _zPosMax);
        }
        if (transform.position.z <= _zPosMin)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _zPosMin);
        }
    }
}
