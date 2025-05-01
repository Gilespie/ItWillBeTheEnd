using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class ChaserEnemy : Enemy
{
    [SerializeField] private float moveSpeed = 5f;

    protected override void Act()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > attackDistance)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
        else
        {
            DealDamage(player);
        }
    }
}

