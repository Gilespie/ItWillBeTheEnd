using UnityEngine;

public class PushableBox : MonoBehaviour, IPushable
{
    [SerializeField]private Player _player;
    //[SerializeField] private Transform _standPos;
    private Rigidbody _rb;
    private float _standMass = 10000f;
    private float _moveMass = 0.001f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.mass = _standMass;
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
        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = _player.GetComponent<Rigidbody>();
        _rb.mass = _moveMass;
    }

    public void StopMoving()
    {
        FixedJoint joint = gameObject.GetComponent<FixedJoint>();
        Destroy(joint);
        _rb.mass = _standMass;
    }
}