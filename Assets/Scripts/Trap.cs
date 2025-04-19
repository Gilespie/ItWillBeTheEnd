using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float _damage = 300f;
    private Player _player;
    private bool _isActive = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (_isActive) return;

        _player = FindObjectOfType<Player>();

        if (_player != null)
        {
            _player.TakeDamage(_damage);
            _player.GetComponent<Rigidbody>().AddForceAtPosition(this.GetComponent<Rigidbody>().velocity * 5f, collision.contacts[0].point, ForceMode.Impulse);
        }

        _isActive = true;
    }

}