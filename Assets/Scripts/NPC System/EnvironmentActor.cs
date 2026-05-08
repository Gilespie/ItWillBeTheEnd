// EnvironmentActor.cs
using UnityEngine;
using UnityEngine.Events;

public class EnvironmentActor : MonoBehaviour, IDirectorActor
{
    [Header("Реакция на действие")]
    public UnityEvent onActionTriggered;

    public void ReceiveCommand(CommandContext context)
    {
        // Проверяем, совпадает ли ожидаемая команда (например, "Toggle")
        if (context.actionID == "Toggle")
        {
            onActionTriggered?.Invoke();
        }
    }
}