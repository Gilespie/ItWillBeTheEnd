using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    //наследовать от энеми от дестрактибл
    [Header("Enemy Settings")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float attackDistance = 2f;
    [SerializeField] protected float detectionDistance = 5f;
    protected float distanceToPlayer;
    protected Rigidbody _rb;
    protected Player player;

    protected virtual void Start()
    {
        player = FindObjectOfType<Player>();
        _rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        if (player == null) return;

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
    }

    protected void FixedUpdate()
    {
        if (distanceToPlayer <= detectionDistance)
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
        // Добавить анимацию ожидания
    }

    protected void DealDamage(Player player)
    {
        player.TakeDamage(damage);
    }
}