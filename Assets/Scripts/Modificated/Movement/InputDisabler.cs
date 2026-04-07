using UnityEngine;

public class InputDisabler : MonoBehaviour
{
    [SerializeField] CharacterInputController _characterInput;
    [SerializeField] CharacterRotator _characterRotator;

    public void DisableControl()
    {
        _characterInput.ToggleComponent();
        _characterRotator.ToggleComponent();
    }

    public void EnableControl()
    {
        _characterInput.ToggleComponent();
        _characterRotator.ToggleComponent();
    }
}