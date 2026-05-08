// DirectorRule.cs (Можно поместить в один файл с PavilionDirector)
using UnityEngine;

[System.Serializable]
public struct DirectorRule
{
    public TriggerSignal signal;
    public GameObject receiver;
    public CommandContext context;
}