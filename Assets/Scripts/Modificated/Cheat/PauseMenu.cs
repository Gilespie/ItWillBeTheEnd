using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button _playButton;
    [SerializeField] Button _exitButton;

    [SerializeField] GameObject _panel;
    bool _isRestarting = false;

    void Awake()
    {
        ChangeState(false);
    }

    void OnEnable()
    {
        EventManager.Subscribe(EventType.OnPaused, ChangeState);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe(EventType.OnPaused, ChangeState);
    }

    void ChangeState(params object[] args)
    {
        if (_isRestarting) return;

        bool value = (bool)args[0];

        Time.timeScale = value ? 0f : 1f;
        _panel.SetActive(value);
    }

    public void RestartLevel()
    {
        if (_isRestarting) return;

        Time.timeScale = 1f;
        LoadSceneManager.Instance.RestartGame();
        _panel.SetActive(false);
        _isRestarting = true;
    }

    public void ExitFromGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}