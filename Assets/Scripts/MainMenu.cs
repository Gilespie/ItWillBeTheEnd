using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _controlsPanel;
    [SerializeField] private GameObject _creditsPanel;

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

    public void ShowCredits()
    {
        _mainMenuPanel.SetActive(false);
        _creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        _creditsPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
    }

    public void ShowControls()
    {
        _mainMenuPanel.SetActive(false);
        _controlsPanel.SetActive(true);
    }

    public void CloseControls()
    {
        _controlsPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
    }

    public void ShowSettings()
    {
        _mainMenuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        _mainMenuPanel.SetActive(true);
        _settingsPanel.SetActive(false);
    }
}