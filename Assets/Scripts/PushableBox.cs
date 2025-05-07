using UnityEngine;

public class PushableBox : MonoBehaviour, IPushable
{
    [SerializeField]private Player _player;
    [SerializeField] private Transform _standPos;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    public void Pushing()
    {
        if (_player.CanMove)
        {
            StartMoving();
        }
        else
        {
            StopMoving();
        }
    }

    public void StartMoving()
    {
        transform.SetParent(_player.transform.GetChild(0));
    }

    public void StopMoving()
    {
        transform.SetParent(null);
    }
}