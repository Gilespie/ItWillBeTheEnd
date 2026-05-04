using UnityEngine;

public class AnimationBridge : MonoBehaviour
{
    private NPCBrain _brain;

    private void Awake()
    {
        // Ищем мозг на родительском объекте (на корне NPC)
        _brain = GetComponentInParent<NPCBrain>();
    }

    // Эту функцию будет вызывать Animator, а мост передаст команду в мозг
    public void OnGreetingFinished()
    {
        if (_brain != null)
        {
            _brain.OnGreetingFinished();
        }
    }
}