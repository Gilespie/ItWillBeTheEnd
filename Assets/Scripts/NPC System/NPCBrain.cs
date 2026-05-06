using UnityEngine;
using UnityEngine.AI;

public class NPCBrain : MonoBehaviour
{
    public enum NPCState { Idle, MovingToPOI, PerformingAtPOI, InteractingWithPlayer }
    private Coroutine _currentActionRoutine;
    
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
        _timer = Random.Range(currentPOI.waitTimeMin, currentPOI.waitTimeMax);
    
        // Запускаем цепочку анимаций
        PlayNextActionAtPOI();
    }

   
    private void PlayNextActionAtPOI()
    {
        if (currentPOI != null && currentPOI.advancedActions.Length > 0)
        {
            int rand = Random.Range(0, currentPOI.advancedActions.Length);
            CinematicAction action = currentPOI.advancedActions[rand];
        
            // Обязательно сохраняем ссылку на корутину, чтобы уметь её убивать!
            _currentActionRoutine = StartCoroutine(PlayCinematicAction(action));
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
    private System.Collections.IEnumerator PlayCinematicAction(CinematicAction action)
    {
        // 1. Проигрываем Intro
        if (!string.IsNullOrEmpty(action.introState))
        {
            yield return StartCoroutine(PlayStateAndWait(action.introState));
        }

        // 2. Проигрываем Loop
        if (!string.IsNullOrEmpty(action.loopState))
        {
            int targetLoops = Random.Range(action.minLoops, action.maxLoops + 1);
            _animator.CrossFadeInFixedTime(action.loopState, 0.25f);
            yield return new WaitForSeconds(0.25f); 

            float singleLoopLength = _animator.GetCurrentAnimatorStateInfo(0).length;
            float totalWaitTime = (singleLoopLength * targetLoops) - 0.25f;

            if (totalWaitTime > 0) yield return new WaitForSeconds(totalWaitTime);
        }

        // 3. Проигрываем Outro
        if (!string.IsNullOrEmpty(action.outroState))
        {
            yield return StartCoroutine(PlayStateAndWait(action.outroState));
        }

        // 4. Если таймер нахождения в точке еще не вышел, запускаем следующий случайный экшен!
        if (currentState == NPCState.PerformingAtPOI && _timer > 0)
        {
            PlayNextActionAtPOI();
        }
    }

// Вспомогательный метод ожидания (из вашего кода)
    private System.Collections.IEnumerator PlayStateAndWait(string stateName)
    {
        _animator.CrossFadeInFixedTime(stateName, 0.25f);
        yield return new WaitForSeconds(0.25f);

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        float timeToWait = stateInfo.length - 0.25f;

        if (timeToWait > 0) yield return new WaitForSeconds(timeToWait);
    }
    public void Command_ReactToPlayer()
    {
        // УБИВАЕМ текущую цепочку анимаций, чтобы NPC всё бросил!
        if (_currentActionRoutine != null)
        {
            StopCoroutine(_currentActionRoutine);
            _currentActionRoutine = null;
        }

        _agent.isStopped = true; 
        currentState = NPCState.InteractingWithPlayer;

        // Машем рукой
        if (greetingAnimations.Length > 0)
        {
            int rand = Random.Range(0, greetingAnimations.Length);
            _animator.CrossFade(greetingAnimations[rand], 0.1f);
        }
    
        // ПРИМЕЧАНИЕ: Здесь можно использовать тот же PlayStateAndWait, 
        // чтобы избавиться от Animation Events даже при приветствии!
    }
}