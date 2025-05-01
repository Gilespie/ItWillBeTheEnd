using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float attackDistance = 2f;
    [SerializeField] protected float detectionDistance = 5f;
    protected Player player;

    protected virtual void Start()
    {
        player = FindObjectOfType<Player>();
    }

    protected virtual void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

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
