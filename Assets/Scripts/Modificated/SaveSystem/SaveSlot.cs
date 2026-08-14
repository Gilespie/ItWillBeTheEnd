using System.IO;
using UnityEngine;

public class SaveSlot<T> where T : new()
{
    private readonly string _path;
    public T Data { get; private set; }

    public SaveSlot(string fileName)
    {
        _path = Path.Combine(Application.persistentDataPath, fileName);
        Data = new T();
    }

    public bool Exists => File.Exists(_path);

    public void Save()
    {
        string json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(_path, json);
    }

    public void Load()
    {
        Data = Exists
            ? JsonUtility.FromJson<T>(File.ReadAllText(_path))
            : new T();
    }
}