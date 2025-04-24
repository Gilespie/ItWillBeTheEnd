using UnityEngine;

public class Acceleration : MonoBehaviour
{
    [SerializeField] private float _speedMultiplier = 3f;
    private float _dafaultSpeed = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Player player))
        {
            _dafaultSpeed = player.GetSpeed();
            player.ChangeSpeed(_speedMultiplier);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.ChangeSpeed(_dafaultSpeed);
        }
    }
}