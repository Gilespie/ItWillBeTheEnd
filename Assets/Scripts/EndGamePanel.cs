using UnityEngine;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    private Animator _animator;

    private void Awake()
    {
        _panel.SetActive(false);
    }

    private void OnEnable()
    {
        Player.OnEndGame += ShowPanel;
    }

    private void OnDisable()
    {
        Player.OnEndGame -= ShowPanel;
    }

    private void ShowPanel()
    {
        _panel.SetActive(true);
    }
}   