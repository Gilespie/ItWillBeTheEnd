using UnityEngine;
using UnityEngine.AI;

public class NPCBrain : MonoBehaviour
{
    public enum NPCState { Idle, MovingToPOI, PerformingAtPOI, InteractingWithPlayer }
    
    [Header("Компоненты")]
    public NPCState currentState = NPCState.Idle;
    private NavMeshAgent _agent;
    private Animator _animator;
    
    [Header("Память NPC")]
    public PointOfInterest currentPOI; // Куда он идет или где находится
    private float _timer; // Таймер для нахождения в точке

    [Header("Анимации реакции на игрока")]
    public string[] greetingAnimations;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case NPCState.MovingToPOI:
                // Проверяем, дошел ли NPC до точки
                if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
                {
                    ArrivedAtPOI();
                }
                break;

            case NPCState.PerformingAtPOI:
                // NPC тусуется на точке. Ждем таймер.
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    // Время вышло. Здесь можно написать логику ухода в ДРУГУЮ точку
                }
                break;
        }
    }

    // --- МЕТОДЫ ДЛЯ СЦЕНАРИЕВ (Вызываются через Триггеры) ---

    // 1. Команда: Иди в точку
    public void Command_GoToPoint(PointOfInterest targetPoint)
    {
        _agent.isStopped = false;
        currentState = NPCState.MovingToPOI;
        currentPOI = targetPoint;
        _agent.SetDestination(targetPoint.transform.position);
        _animator.CrossFade("Walk", 0.1f); // Включаем анимацию ходьбы
    }

    // Когда дошел до точки
    private void ArrivedAtPOI()
    {
        currentState = NPCState.PerformingAtPOI;
        
        // Устанавливаем случайное время нахождения здесь
        _timer = Random.Range(currentPOI.waitTimeMin, currentPOI.waitTimeMax);
        
        // Берем случайную анимацию из САМОЙ ТОЧКИ
        if (currentPOI.pointAnimations.Length > 0)
        {
            int rand = Random.Range(0, currentPOI.pointAnimations.Length);
            _animator.CrossFade(currentPOI.pointAnimations[rand], 0.25f);
        }
    }

    // 2. Команда: Игрок подошел (Триггер)
    public void Command_ReactToPlayer()
    {
        // Прерываем всё, что NPC делал до этого
        _agent.isStopped = true; 
        currentState = NPCState.InteractingWithPlayer;

        // Поворачиваемся к игроку (опционально)
        
        // Машем рукой
        if (greetingAnimations.Length > 0)
        {
            int rand = Random.Range(0, greetingAnimations.Length);
            _animator.CrossFade(greetingAnimations[rand], 0.1f);
        }
    }
    
    // Вызывается через Animation Event в конце анимации приветствия!
    public void OnGreetingFinished()
    {
        _agent.isStopped = false; // Разрешаем идти дальше
        // Если у него была цель - он продолжит путь
        if (currentPOI != null) Command_GoToPoint(currentPOI);
        else currentState = NPCState.Idle;
    }
}