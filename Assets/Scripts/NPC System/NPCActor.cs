// NPCActor.cs
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCActor : MonoBehaviour, IDirectorActor
{
    private NavMeshAgent _agent;
    private int _currentPriority = -1;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void ReceiveCommand(CommandContext context)
    {
        // Игнорируем команды с меньшим приоритетом
        if (context.priority < _currentPriority) return;
        
        _currentPriority = context.priority;

        if (context.targetPOI != null)
        {
            _agent.isStopped = false;
            _agent.SetDestination(context.targetPOI.position);
            
            // В дальнейшем здесь будет вызов корутины анимаций для новой точки POI
        }
    }
}