using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : SingletonBase<LoadSceneManager>
{
    [SerializeField] float _minLoadingTime = 3f;
    [SerializeField] LoadProgressUI _loaderUI;
    [SerializeField] ScreenFader _fader;
    bool _isLoading = false;

    void OnEnable()
    {
        EventManager.Subscribe(EventType.OnDead, RestartGame);
        EventManager.Subscribe(EventType.OnSceneTransition, LoadScene);
    }

    void Start()
    {
        _fader.FadeIn();
    }

    void OnDisable()
    {
        EventManager.Unsubscribe(EventType.OnDead, RestartGame);
        EventManager.Unsubscribe(EventType.OnSceneTransition, LoadScene);
    }

    public void LoadScene(params object[] args)
    {
        string sceneName = (string)args[0];

        if (!_isLoading)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }
    }

    public void RestartGame(params object[] args)
    {
        string currentLevelname = SceneManager.GetActiveScene().name;

        LoadScene(currentLevelname);
    }

    /*void OnSceneRequested(params object[] args)
    {
        if (args == null || args.Length == 0 || args[0] is not string sceneName)
        {
            Debug.LogError($"{nameof(LoadSceneManager)}: OnSceneFinish без имени сцены");
            return;
        }

        TryLoad(sceneName);
    }

    void OnRestartRequested(params object[] args)
    {
        string currentLevelName = SceneManager.GetActiveScene().name;
        TryLoad(currentLevelName);
    }

    void TryLoad(string sceneName)
    {
        if (_isLoading) return;
        StartCoroutine(LoadSceneAsync(sceneName));
    }*/

    /*IEnumerator LoadSceneAsync(string sceneName)
    {
        _isLoading = true;

        _loaderUI.Show();
       
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
        asyncOp.allowSceneActivation = false;

        float fakeProgress = 0f;

        while (asyncOp.progress < 0.9f)
        {
            float target = asyncOp.progress / 0.9f;

            fakeProgress = Mathf.MoveTowards(
                fakeProgress,
                target,
                Time.deltaTime * _loadingSpeed
            );

            _loaderUI.SetProgress(fakeProgress);
            yield return null;
        }

        while (fakeProgress < 1f)
        {
            fakeProgress = Mathf.MoveTowards(
                fakeProgress,
                1f,
                Time.deltaTime * _loadingSpeed
            );

            _loaderUI.SetProgress( fakeProgress );
            yield return null;
        }

        asyncOp.allowSceneActivation = true;

        while (!asyncOp.isDone)
            yield return null;

        _loaderUI.Hide();

        _isLoading = false;
    }*/

    IEnumerator LoadSceneAsync(string sceneName)
    {
        _isLoading = true;

        yield return _fader.FadeOut();

        _loaderUI.ShowText();

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
        asyncOp.allowSceneActivation = false;

        float elapsedTime = 0f;

        while (asyncOp.progress < 0.9f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        while (elapsedTime < _minLoadingTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        asyncOp.allowSceneActivation = true;

        while (!asyncOp.isDone)
            yield return null;

        _loaderUI.HideText();

        yield return _fader.FadeIn();

        _isLoading = false;
    }
}