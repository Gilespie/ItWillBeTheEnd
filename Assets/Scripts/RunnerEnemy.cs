using UnityEngine;

public class RunnerEnemy : Enemy
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float collisionDamage = 5f;

    private bool hasSeenPlayer = false;
    private Vector3 moveDirection;

    protected override void Act()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (!hasSeenPlayer && distance <= detectionRadius)
        {
            hasSeenPlayer = true;
            moveDirection = (transform.position - player.transform.position).normalized;
        }

        if (hasSeenPlayer)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player hitPlayer))
        {
            DealDamage(hitPlayer);
        }
    }
}
