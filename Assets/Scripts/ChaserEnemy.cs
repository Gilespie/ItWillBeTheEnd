using UnityEngine;

public class ChaserEnemy : Enemy
{
/*    private void OnEnable()
    {
        player.OnDead += Act;
    }

    private void OnDisable()
    {
        player.OnDead -= Act;
    }*/

    protected override void Act()
    {
        if (player.IsAlive == false) return;

        if (distanceToPlayer > attackDistance)
        {
            _animator.SetFloat(_nameIdle, 1f);
            _animator.SetFloat(_nameRun, 1f);
            RotateTransform(player.transform);
            Vector3 dir = (player.transform.position - transform.position).normalized;
            _rb.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime);
        }
        else
        {
            DealDamage(player);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
    }
}