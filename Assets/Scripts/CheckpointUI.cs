using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointUI : MonoBehaviour
{
    [SerializeField] private Image _saveIcon;
    [SerializeField] private Checkpoint[] _points;
    private Animation _animation;

    private void OnEnable()
    {
        for (int i = 0; i < _points.Length; i++)
        {
            _points[i].OnCheckpoint += OnCheckpoint;
        }
    }

    private void Start()
    {
        _animation = GetComponentInChildren<Animation>();
        _saveIcon.color = new(_saveIcon.color.r, _saveIcon.color.g, _saveIcon.color.b, 0);
    }

    private void OnDisable()
    {
        for (int i = 0; i < _points.Length; i++)
        {
            _points[i].OnCheckpoint -= OnCheckpoint;
        }
    }

    private void OnCheckpoint()
    {
        _animation.Stop();
        _animation.Play();
    }
}