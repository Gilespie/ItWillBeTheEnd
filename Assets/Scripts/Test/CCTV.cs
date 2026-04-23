using UnityEngine;

public class CCTV : MonoBehaviour
{
    [SerializeField] RenderTexture m_Texture;
    [SerializeField] Transform _target;

    private void Update()
    {
        if (_target == null) return;
        Vector3 direction = _target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }
}