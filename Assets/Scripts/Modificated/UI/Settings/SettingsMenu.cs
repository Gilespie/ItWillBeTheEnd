using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [Header("Selectors")]
    [SerializeField] private OptionSelector _resolutionSelector;
    [SerializeField] private OptionSelector _qualitySelector;
    [SerializeField] private OptionSelector _vSyncSelector;
    [SerializeField] private OptionSelector _fullscreenSelector;
    [SerializeField] private List<Resolution> _resolutions = new();

    void Awake()
    {
        InitResolutionSelector();
        InitQualitySelector();
        InitVSyncSelector();
        InitFullscreenSelector();

        _resolutionSelector.OnValueChanged += ChangeResolution;
        _qualitySelector.OnValueChanged += ChangeQuality;
        _vSyncSelector.OnValueChanged += ChangeVSync;
        _fullscreenSelector.OnValueChanged += ChangeFullscreen;

    }

    private void OnDestroy()
    {
        _resolutionSelector.OnValueChanged -= ChangeResolution;
        _qualitySelector.OnValueChanged -= ChangeQuality;
        _vSyncSelector.OnValueChanged -= ChangeVSync;
        _fullscreenSelector.OnValueChanged -= ChangeFullscreen;
    }

    void InitResolutionSelector()
    {
        List<string> values = new();

        foreach (Resolution res in Screen.resolutions)
        {
            string text = $"{res.width} x {res.height}";

            if (values.Contains(text))
                continue;

            values.Add(text);
            _resolutions.Add(res);
        }

        int currentIndex = 0;

        for (int i = 0; i < _resolutions.Count; i++)
        {
            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentIndex = i;
                break;
            }
        }

        _resolutionSelector.Initialize(values, currentIndex);
    }

    void InitQualitySelector()
    {
        List<string> quality = new(QualitySettings.names);
        _qualitySelector.Initialize(quality, SaveManager.Instance.Data.QualityIndex);
    }

    void InitVSyncSelector()
    {
        _vSyncSelector.Initialize(new List<string>{"OFF", "ON"}, SaveManager.Instance.Data.VSync);
    }

    void InitFullscreenSelector()
    {
        _fullscreenSelector.Initialize(new List<string>{"OFF", "ON"}, SaveManager.Instance.Data.Fullscreen ? 1 : 0);
    }

    void InitVolume()
    {

    }

    void ChangeResolution(int index)
    {
        SaveManager.Instance.Data.ResolutionIndex = index;

        Resolution res = _resolutions[index];

        Screen.SetResolution(
            res.width,
            res.height,
            SaveManager.Instance.Data.Fullscreen);

        SaveManager.Instance.Save();
    }

    void ChangeQuality(int index)
    {
        SaveManager.Instance.Data.QualityIndex = index;
        SaveManager.Instance.Apply();
        SaveManager.Instance.Save();
    }

    void ChangeFullscreen(int index)
    {
        SaveManager.Instance.Data.Fullscreen = index == 1;

        SaveManager.Instance.Apply();
        SaveManager.Instance.Save();
    }

    void ChangeVSync(int index)
    {
        SaveManager.Instance.Data.VSync = index;

        SaveManager.Instance.Apply();
        SaveManager.Instance.Save();
    }

    void ChangeVolume(int value)
    {

    }
}