using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    //наследовать энеми от дестрактибл
    [Header("Animations")]
    [SerializeField] protected string _nameIdle = "Vert"; 
    [SerializeField] protected string _nameRun = "State"; 
    [SerializeField] protected Transform _mesh;

    [Header("Enemy Settings")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float rotationSpeed = 10f;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float attackDistance = 2f;
    [SerializeField] protected float detectionDistance = 5f;

    protected float distanceToPlayer;
    protected Animator _animator;
    protected Rigidbody _rb;
    protected Destructable player;
    protected Vector3 _direction;

    protected virtual void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        player = GameManager.Instance.Player;
        _rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        if (player == null) return;

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
    }

    protected void FixedUpdate()
    {
        if (distanceToPlayer <= detectionDistance && player.IsAlive)
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
        _animator.SetFloat(_nameIdle, 0f);
        _animator.SetFloat(_nameRun, 0f);
    }

    protected void DealDamage(Destructable destructable)
    {
        destructable.TakeDamage(damage);
    }

    protected void RotateTransform(Transform target)
    {
        _direction = target.position - transform.position;

        // Убираем влияние по оси Y (смотрим только в горизонтальной плоскости)
        _direction.y = 0;

        if (_direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            _mesh.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}