using UnityEngine;

public class InitialGame : MonoBehaviour
{
    [SerializeField] AnimationCurve _alphaText;
    [SerializeField] TMPro.TextMeshProUGUI _text;
    [SerializeField] float _duration = 1f;
    [SerializeField] bool _isStarted = false;
    float _time = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        _time += Time.deltaTime * _duration;

        float duration = _alphaText.keys[_alphaText.length - 1].time;
        float t = Mathf.Repeat(_time, duration);

        float normalized = _alphaText.Evaluate(t);
        _text.alpha = normalized;
    }
}