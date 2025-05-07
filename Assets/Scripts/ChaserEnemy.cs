using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserEnemy : Enemy
{
    [SerializeField] private float moveSpeed = 5f;

    protected override void Act()
    {
        if (distanceToPlayer > attackDistance)
        {
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