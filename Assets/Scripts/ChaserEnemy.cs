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

            /*RotateTransform(_player.transform);
            Vector3 dir = (_player.transform.position - transform.position).normalized;
            _rb.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime);*/
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