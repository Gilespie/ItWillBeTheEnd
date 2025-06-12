using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public abstract class Enemy : MonoBehaviour
{
    //наследовать энеми от дестрактибл
    [Header("Animations")]
    [SerializeField] protected string _nameIdle = "Vert"; 
    [SerializeField] protected string _nameRun = "State"; 
    //[SerializeField] protected Transform _mesh;

    [Header("Enemy Settings")]
    [SerializeField] protected float damage = 100f;
    /*[SerializeField] protected float rotationSpeed = 10f;
    [SerializeField] protected float moveSpeed = 5f;*/
    [SerializeField] protected float attackDistance = 2f;
    [SerializeField] protected float detectionDistance = 5f;
    [SerializeField] protected float _updateNodeDistance = 0.75f;

    protected Transform[] _aiNodes;
    protected Transform _actualNode;
    protected float _distanceToPlayer, _distanceToNode;
    protected Animator _animator;
    protected Rigidbody _rb;
    protected Destructable _player;
    protected Vector3 _direction;
    protected NavMeshAgent _agent;

    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody>();
        _player = GameManager.Instance.Player;
        _aiNodes = GameManager.Instance.AIDogNodes;

        _actualNode = GetNewNode();

        _agent.SetDestination(_actualNode.position);
    }

    protected virtual void Update()
    {
        if (_player == null) return;

        _animator.SetFloat(_nameIdle, _agent.velocity.magnitude);
        _animator.SetFloat(_nameRun, _agent.velocity.magnitude);

        _distanceToPlayer = Vector3.SqrMagnitude(transform.position - _player.transform.position);

        if (_distanceToPlayer <= detectionDistance * detectionDistance && _player.IsAlive)
        {
            Act();
        }
        else
        {
            Idle();
        }
    }

    protected abstract void Act();

    protected virtual void Idle()
    {
        if (_agent.destination != _actualNode.position)
        {
            _agent.SetDestination(_actualNode.position);
        }
        
        _distanceToNode = Vector3.SqrMagnitude(transform.position - _actualNode.position);

        if (_distanceToNode <= _updateNodeDistance * _updateNodeDistance)
        {
            _actualNode = GetNewNode(_actualNode);
            _agent.SetDestination(_actualNode.position);
        }
    }

    protected void DealDamage(Destructable destructable)
    {
        destructable.TakeDamage(damage);
    }

    private Transform GetNewNode(Transform actual = null)
    {
        if(!actual)
        {
            return _aiNodes[Random.Range(0, _aiNodes.Length)];
        }
        else
        {
            Transform newNode;

            do
            {
                newNode = _aiNodes[Random.Range(0, _aiNodes.Length)];
            }
            while (actual == newNode);

            return newNode;
        }
    }

    /*protected void RotateTransform(Transform target)
    {
        _direction = target.position - transform.position;

        _direction.y = 0;

        if (_direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            _mesh.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }*/
}