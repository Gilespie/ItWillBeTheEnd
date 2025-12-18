using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    #region Singleton
    public static LoadSceneManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("UI")]
    [SerializeField] private Image _loadingBG;
    [SerializeField] private float _fadeTime = 0.5f;
    [SerializeField] private float _loadingSpeed = 0.3f;
    [SerializeField] private TextMeshProUGUI _stateText;
    [SerializeField] private Slider _sliderProgress;
    private bool _isLoading = false;

    private void Start()
    {
        _sliderProgress.value = 0.0f;
        _stateText.text = $"";

        _loadingBG.enabled = false;
        _stateText.enabled = false;
        _sliderProgress.gameObject.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        if (!_isLoading)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        _isLoading = true;

        _loadingBG.enabled = true;

        float t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime / _fadeTime;
            _loadingBG.color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 1f, t));
            yield return null;
        }

        _loadingBG.color = new Color(0f, 0f, 0f, 1f);

        _sliderProgress.gameObject.SetActive(true);

        _stateText.enabled = true;
        _stateText.text = "Loading...";

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

            _sliderProgress.value = fakeProgress;
            yield return null;
        }

        while (fakeProgress < 1f)
        {
            fakeProgress = Mathf.MoveTowards(
                fakeProgress,
                1f,
                Time.deltaTime * _loadingSpeed
            );

            _sliderProgress.value = fakeProgress;
            yield return null;
        }

        asyncOp.allowSceneActivation = true;

        while (!asyncOp.isDone)
            yield return null;

        _stateText.enabled = false;
        _sliderProgress.gameObject.SetActive(false);

        t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime / _fadeTime;
            _loadingBG.color = new Color(0f, 0f, 0f, Mathf.Lerp(1f, 0f, t));
            yield return null;
        }

        _loadingBG.color = new Color(0f, 0f, 0f, 0f);

        _isLoading = false;
    }
}