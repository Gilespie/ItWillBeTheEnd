using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] private float _fadeTime = 2f;
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        ActivateFadeOut();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void ActivateFadeIn()
    {
        StartCoroutine(FadeInRoutine());
    }

    public void ActivateFadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        float elapsed = 0f;
        Color color = _image.color;

        while (elapsed < _fadeTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(0f + elapsed / _fadeTime);
            color.a = alpha;
            _image.color = color;
            yield return null;
        }

        color.a = 1f;
        _image.color = color;

        RestartGame();
    }

    private IEnumerator FadeOutRoutine()
    {
        float elapsed = 0f;
        Color color = _image.color;

        while (elapsed < _fadeTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - elapsed / _fadeTime);
            color.a = alpha;
            _image.color = color;
            yield return null;
        }

        color.a = 0f;
        _image.color = color;
    }
}