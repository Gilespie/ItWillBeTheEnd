using UnityEngine;

public class ChaserEnemy : Enemy
{
    protected override void Act()
    {
        if (_player.IsAlive == false) return;

        if (_distanceToPlayer > attackDistance * attackDistance)
        {
            _animator.SetFloat(_nameIdle, 1f);
            _animator.SetFloat(_nameRun, 1f);

            if(_agent.isStopped)
            {
                _agent.isStopped = true;
            }

            _agent.SetDestination(_player.transform.position);
        }
        else
        {
            if (!_agent.isStopped)
            {
                _agent.isStopped = true;
            }

            _animator.SetFloat(_nameIdle, 0f);
            _animator.SetFloat(_nameRun, 0f);

            DealDamage(_player);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
    }
}