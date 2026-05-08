// CommandContext.cs
using UnityEngine;

[System.Serializable]
public struct CommandContext
{
    public Transform targetPOI;
    public string actionID;
    public int priority;
}