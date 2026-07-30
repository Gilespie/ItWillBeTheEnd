using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveManager : SingletonBase<SaveManager>
{
    InputsActions _inputActions;
    public InputsActions InputActions => _inputActions;

    public SettingsData Data = new();
    string SavePath => Path.Combine(Application.persistentDataPath, "settings.json");

    protected override void Awake()
    {
        base.Awake();

        _inputActions = new InputsActions();

        Load();
        Apply();
        ApplyInputBindings();
    }

    public void Save()
    {
        SaveInputBindings();

        string json = JsonUtility.ToJson(Data, true);

        File.WriteAllText(SavePath, json);
    }

    public void Load()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            Data = JsonUtility.FromJson<SettingsData>(json);
        }
        else
        {
            Data = new SettingsData();
            Save();
        }
    }

    public void Apply()
    {
        QualitySettings.SetQualityLevel(Data.QualityIndex);
        QualitySettings.vSyncCount = Data.VSync;
        Screen.fullScreen = Data.Fullscreen;
    }

    public void SaveInputBindings()
    {
        Data.InputBindingsOverridesJson = _inputActions.asset.SaveBindingOverridesAsJson();
    }

    public void ApplyInputBindings()
    {
        if (!string.IsNullOrEmpty(Data.InputBindingsOverridesJson))
        {
            _inputActions.asset.LoadBindingOverridesFromJson(Data.InputBindingsOverridesJson);
        }
    }
}