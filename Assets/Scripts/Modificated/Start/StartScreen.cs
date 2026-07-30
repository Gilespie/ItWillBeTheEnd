using UnityEngine;

public class StartScreen : MonoBehaviour
{
    [SerializeField] Animator _playerAnimator;
    bool _started;

    void OnEnable()
    {
        EventManager.Subscribe(EventType.OnStartGame, StartGame);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe(EventType.OnStartGame, StartGame);
    }

    void StartGame(params object[] args)
    {
        if (_started)
            return;

        _started = true;

        gameObject.SetActive(false);

        _playerAnimator.SetTrigger("StandUp");
    }
}